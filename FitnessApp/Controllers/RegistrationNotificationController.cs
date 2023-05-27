using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Data;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using FitnessApp.Utility;
using FitnessApp.DTOs;
using Infobip.Api.Client.Api;
using Infobip.Api.Client.Model;
using Infobip.Api.Client;
using Microsoft.AspNetCore.Identity;

namespace FitnessApp.Controllers
{
    public class RegistrationNotificationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GetCurrentUserProperties _getCurrentUserProperties;
        private readonly UserManager<Person> _userManager;
        private static readonly string BASE_URL = "https://198j8x.api.infobip.com";
        private static readonly string API_KEY = "61af7a6525013e553eee9dbedc906675-1065bab4-9405-4e6b-bdb2-f655638b2347";
        private static readonly string SENDER = "InfoSMS";
        private static readonly string RECIPIENT = "962780388117";
        private static string MESSAGE_TEXT = "";
        public RegistrationNotificationController(ApplicationDbContext context,
            GetCurrentUserProperties getCurrentUserProperties,
            UserManager<Person> userManager)
        {
            _context = context;
            _getCurrentUserProperties = getCurrentUserProperties;
            _userManager = userManager;
        }
        [Authorize(Roles =Roles.Admin)]
        // GET: RegistrationNotification
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.RegistrationNotifications
                .Include(r => r.GymBundle)
                .Include(a => a.Trainer)
                .ThenInclude(q => q.Person)
                .ToListAsync();
            return View(applicationDbContext);

            //var obj = await(from register in _context.RegistrationNotifications
            //           join gym in _context.GymBundles on register.GymBundleId equals gym.Id
            //           join trainer in _context.Trainers on register.TrainerId equals trainer.Id
            //           join person in _context.Users on trainer.PersonId equals person.Id
            //           select (new RegistrationNotificationsDTO 
            //           {
            //           Age = register.Age,
            //           Email =register.Email,
            //           Gender =register.Gender,
            //           GymBundleTitle = gym.BundleTitle,
            //           Height =register.Height,
            //           PhoneNumber =register.PhoneNumber,
            //           Trainer = person.Email,
            //           Weight=register.Weight
            //           })).ToListAsync();
            //        return View(obj);

        }

        // GET: RegistrationNotification/Create
        public IActionResult Create()
        {
            ViewData["GymBundleId"] = new SelectList(_context.GymBundles, "Id", "BundleTitle");
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "Id");
            return View();
        }

        // POST: RegistrationNotification/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( RegistrationNotification registrationNotification)
        {
            if (ModelState.IsValid)
            {
                string password = "User1234*";
                var result = _getCurrentUserProperties.CheckUserExistance(registrationNotification.Email);
                if (result )
                {
                    ModelState.AddModelError("", "Sorry the user is exist ");
                    return View(registrationNotification);
                }
                registrationNotification.Id = Guid.NewGuid().ToString();
                var firstname = registrationNotification.Email.Substring(0, registrationNotification.Email.IndexOf("@"));
                _context.Add(registrationNotification);
                await _context.SaveChangesAsync();
                SendToBecomeAMemberMessage.BecomeAMemberMessage();
                var username = registrationNotification.Email.Substring(0, registrationNotification.Email.IndexOf("@"));
                var user = new Person
                {
                     Email =registrationNotification.Email,
                     Gender = registrationNotification.Gender,
                     EmailConfirmed =true,
                     PhoneNumber =registrationNotification .PhoneNumber,
                     PhoneNumberConfirmed = true,
                     RegisterDate = DateTime.Now,
                     Role =Roles.Member,
                     UserName = registrationNotification.Email,
                     UserAvatar = $"https://ui-avatars.com/api/?name={username}"
                };

                var check = await _userManager.FindByEmailAsync(registrationNotification.Email);
                if (check != null)
                {
                    ModelState.AddModelError("", "Email is already used");
                    return View(registrationNotification);
                }
                

                    // Create person
                  _userManager.CreateAsync(user, password).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(user,Roles.Member).GetAwaiter().GetResult();


                var memeberDTO = new WeightCalculateDTO
                {
                    Weight = registrationNotification.Weight,
                    Height = registrationNotification.Height,
                    Age = registrationNotification.Age
                };
                // get gymbundle
                var gym = _context.GymBundles.Where(q => q.Id == registrationNotification.GymBundleId).FirstOrDefault();
                var date = DateTime.Now;
                // Create member
                var member = new Member
                {
                    Id = Guid.NewGuid().ToString(),
                    Age = registrationNotification.Age,
                    Height = registrationNotification.Height,
                    DateOfBirth = registrationNotification.DateOfBirth,
                    GymBundle = registrationNotification.GymBundle,
                    PersonId = user.Id,
                    Weight = registrationNotification.Weight,
                    BMIStatus= WeightState.SetWeightStatus(registrationNotification.Height, registrationNotification.Weight),
                    ExpectedWeight = CalculateWeight.GetPerfectWeight(memeberDTO),
                    IsMemberOverWeight = CalculateWeight.IsOverweight(memeberDTO),
                    MembershipFrom = date,
                };
                member.MembershipTo = member.MembershipFrom.AddDays((double)gym.NumberOfDays);
                _context.Members.Add(member);
                await _context.SaveChangesAsync();


                var trainer = _context.Trainers.Where(q => q.Id == registrationNotification.TrainerId).FirstOrDefault();

                // add Trainers Members
                var trainermember = new TrainersMember()
                {
                    Id = Guid.NewGuid().ToString(),
                    MemberId = member.Id,
                    TrainerId = trainer.Id,
                };
                _context.TrainersMembers.Add(trainermember);
                await _context.SaveChangesAsync();
                SendRegistrationNotificationMessageSMS(firstname, registrationNotification.Email, password);
                return RedirectToAction(nameof(Index),"Home");
            }
            return View(registrationNotification);
        }


        public void SendRegistrationNotificationMessageSMS(string firstname, string username, string password )
        {
            // message ="Dear Member {username} Welocme to Fitness Training your username:{Username }
            // and your password :{password}"
            Task.Delay(10000).GetAwaiter().GetResult();
            MESSAGE_TEXT = $"Dear ${ firstname} Thank you for registering with us. We welcome you as a new member and wish you success." +
                $"Your Username : ${ username} " +
                $"Your password : ${ password} ";


            var configuration = new Configuration()
            {
                BasePath = BASE_URL,
                ApiKeyPrefix = "App",
                ApiKey = API_KEY
            };

            var sendSmsApi = new SendSmsApi(configuration);

            var smsMessage = new SmsTextualMessage()
            {
                From = SENDER,
                Destinations = new List<SmsDestination>()
                {
                    new SmsDestination(to: RECIPIENT)
                },
                Text = MESSAGE_TEXT
            };

            var smsRequest = new SmsAdvancedTextualRequest()
            {
                Messages = new List<SmsTextualMessage>() { smsMessage }
            };

            try
            {
                var smsResponse = sendSmsApi.SendSmsMessage(smsRequest);

                Console.WriteLine("Response: " + smsResponse.Messages.FirstOrDefault());
            }
            catch (ApiException apiException)
            {
                Console.WriteLine("Error occurred! \n\tMessage: {0}\n\tError content", apiException.ErrorContent);
            }

        }






        //// GET: RegistrationNotification/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var registrationNotification = await _context.RegistrationNotifications
        //        .Include(r => r.GymBundle)
        //        .Include(r => r.Trainer)
        //        .Include(q => q.Trainer.Person)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (registrationNotification == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["GymBundleId"] = new SelectList(_context.GymBundles, "Id", "BundleTitle", registrationNotification.GymBundleId);
        //    ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "Peron.Email", registrationNotification.TrainerId);
        //    return View(registrationNotification);
        //}

        //// POST: RegistrationNotification/Edit/5
        //[Authorize(Roles = Roles.Admin)]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, RegistrationNotification registrationNotification)
        //{
        //    if (id != registrationNotification.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(registrationNotification);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!RegistrationNotificationExists(registrationNotification.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(registrationNotification);
        //}

        //// GET: RegistrationNotification/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var registrationNotification = await _context.RegistrationNotifications
        //        .Include(r => r.GymBundle)
        //        .Include(r => r.Trainer)
        //        .Include(q => q.Trainer.Person)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (registrationNotification == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(registrationNotification);
        //}

        //// POST: RegistrationNotification/Delete/5
        //[Authorize(Roles = Roles.Admin)]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    var registrationNotification = await _context.RegistrationNotifications.FindAsync(id);
        //    _context.RegistrationNotifications.Remove(registrationNotification);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        //// GET: RegistrationNotification/Details/5
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var registrationNotification = await _context.RegistrationNotifications
        //        .Include(r => r.GymBundle)
        //        .Include(r => r.Trainer)
        //       .Include(q => q.Trainer.Person)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (registrationNotification == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(registrationNotification);
        //}







        private bool RegistrationNotificationExists(string id)
        {
            return _context.RegistrationNotifications.Any(e => e.Id == id);
        }
    }
}
