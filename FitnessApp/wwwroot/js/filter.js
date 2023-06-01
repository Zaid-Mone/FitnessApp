




function filterData() {
    var selectedDate = document.getElementById("dateFilter").value;
    axios.get('/API/GetMyNutraitonByParam', {
        params: {
            filter: selectedDate
        }
            .then(function (response) {
                // handle success\
                debugger;
                var container = document.getElementById("dataContainer");
                container.innerHTML = "";

                // Update the container element with the new data
                container.innerHTML = response;
                console.log(response);
            })
            .catch(function (error) {
                // handle error
                console.log(error);
            })

    }
}
