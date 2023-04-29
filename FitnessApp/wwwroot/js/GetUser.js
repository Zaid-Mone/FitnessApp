
document.addEventListener('DOMContentLoaded', function () {
    // your JavaScript code here
    axios.get('/Member/GetTrainers')
        .then(function (response) {
            // handle success
            const trainer = document.getElementById('selectTrainer');
            response.data.forEach(customer => {
                const option = document.createElement('option');
                option.value = customer.id;
                option.textContent = customer.email;
                trainer.appendChild(option);
            });
            console.log(response);
        })
        .catch(function (error) {
            // handle error
            console.log(error);
        })
});

