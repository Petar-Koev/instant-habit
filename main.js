    async function addHabit(){
        let newHabitName = document.getElementById("new-habit-input").value;

        let request = {name: newHabitName};
        let response = await instantHabitApi.habits.addHabit(request);

        checkForHabitDuplicates(response);
    }

    window.addEventListener('load',displayHabits);

    async function GetAllHabits(){

        let response = await instantHabitApi.habits.getAllHabits();

        if(response.succeeded == false){
          
            alert("Something went wrong");
            return;
        }

        return response.habits;
    }

    async function displayHabits(){

        let allHabits = await getInfoFromDB(GetAllHabits());
        let habitsStructure = await createHabits(allHabits);

        if(allHabits.length === 0) {
            habitsStructure = "";
            return;
        }

        document.getElementById("habits-area").innerHTML = habitsStructure;

        await setListenersForDeleteBtns();
        await setListenersForOpenBtns();
    }

    async function createHabits(allHabits){
        let habitsStructure = `
        <div id="habits">
            <h2 id="habits-name-h2">Habits:</h2>
            <div id="all-habits">`;

        for(let i = 0; i <allHabits.length; i++){
            habitsStructure += `
            <div class="habit" id="habit-${allHabits[i].id}">
            <div class="content">
                <p class="text"
                value="${allHabits[i].id}"> 
                ${allHabits[i].name}
                </p>
            </div>
            <div class="actions">
                <button class="open" value="${allHabits[i].name}-${allHabits[i].id}">Open</button>
                <button class="delete" value="${allHabits[i].id}">Delete</button>
            </div>
            </div>
            `;
        }
        habitsStructure += `
            </div>
        </div>`;

        return habitsStructure;
    }

    function splitValues(item){

        let array = item.split("-");
        let first = array[0];
        let second = array[1];
        let splitObj = {text: first, id: second}

        return splitObj;
    }

    async function openHabit(value){

        let values = splitValues(value);
        
        document.getElementById("opened-habit-id").value = values.id;
        document.getElementById("habit-name-h2").innerHTML = values.text;
        
        let habitArea = document.getElementById("habit-area");

        habitArea.style.display = "block";
        document.getElementById("daily-description-text").style.display = "none";

        await getDescription();
        await handelIsExtended();
        await displayDays();
        await getStreak();
    }

    async function DeleteHabit(e){
        e = e || window.event;
        let habitArea = document.getElementById("habit-area");
        let habitToDelete = document.getElementById(`habit-${e.target.value}`);

        let allHabits = await getInfoFromDB(GetAllHabits());

        for(let i = 0; i < allHabits.length; i++){
            if(allHabits[i].id == e.target.value){
            await deleteSQLHabit(e);
            habitToDelete.remove();
            habitArea.style.display = "none";
            }
        }

         if(allHabits.length == 1){
            document.getElementById("habits-name-h2").innerHTML = "";
         }
    }

    async function setListenersForOpenBtns(){
        let openBtns = document.getElementsByClassName("open");

        for(let i = 0; i < openBtns.length; i++){
            openBtns[i].addEventListener("click", async function(e){
            await openHabit(openBtns[i].value);
            await displayDays();
            });
        }
    }

    async function setListenersForDeleteBtns(){
        let deleteBtns = document.getElementsByClassName("delete");

        for(let i = 0; i < deleteBtns.length; i++){
            deleteBtns[i].addEventListener("click", async function(e){
            await DeleteHabit(e);
            });
        }
    }

    async function deleteSQLHabit(e){
        e = e || window.event;

        let habitToDelete = e.target.value;
        let request = {id: habitToDelete};

        let response = await instantHabitApi.habits.deleteSQLHabit(request);

        if(response.succeeded == true){
            await displayHabits();  
        }
        else {
            console.log(response);
        }
    }

    async function addDescription() {

        let newDescription = document.getElementById("description-input").value;

        if(newDescription == ""){
            alert("Please enter a description");
        } else {
            let id = document.getElementById("opened-habit-id").value;
            let request = {habitId: id, description: newDescription};

            let response = await instantHabitApi.habits.addDescription(request);
            await descriptionResponseCheck(response);
        } 
    }

    async function descriptionResponseCheck(response){
        if(response.succeeded == true){
            document.getElementById("description-input").value = "";
            await getDescription();
        }
        else {
            console.log(response);
        }
    }

    async function getHabitById(){

        let habitId = document.getElementById("opened-habit-id").value;
        let response = await instantHabitApi.habits.getHabitById(habitId);

        if(response.succeeded == false){
            alert("Something went wrong");
            return;
        }

        return response.habit;
    }

    async function getDescription(){

        let selectedHabit = await getInfoFromDB(getHabitById());
        console.log(selectedHabit.habit);

        document.getElementById("description-text").innerHTML = selectedHabit.description;
    }

    async function getAllHabitDays(){

        let habitId = document.getElementById("opened-habit-id").value;
        let response = await instantHabitApi.days.getAllHabitDays(habitId);

        if(response.succeeded == false){
            alert("Something went wrong");
            return;
        }
        return  response.days;
        
    }

    async function displayDays(){

        refreshDays();
        let days = await getInfoFromDB(getAllHabitDays());

        for (let i = 0; i < days.length; i++){
            let probe = document.getElementById(`day-${days[i].dayNumber}`);
            probe.style.color = "red";
        }
    }

    async function addDay(id){

        let currentHabitId = document.getElementById("opened-habit-id").value;
        let selectedBtn = document.getElementById(id);

        let values = splitValues(id);

        let request = {habitId: currentHabitId, dayNumber: values.id};

        let response = await instantHabitApi.days.addDay(request);

        if(response.succeeded == true){
            await updateSelectedDay(values); 
        } else if (response.succeeded == false){
            await dayRemoval(currentHabitId, values, selectedBtn);
        }
    }

    async function dayRemoval(currentHabitId, values, selectedBtn){
        await deleteDay(currentHabitId,values.id);
            selectedBtn.style.color = "white";
            await getStreak();
            document.getElementById("daily-description-text").style.display = "none";
    }

    async function updateSelectedDay(values){
        document.getElementById("active-day-id").value = values.id;
            await displayDays();
            await getStreak();
            document.getElementById("daily-description-text").style.display = "none";
            dayDescriptionAlert();
               if(document.getElementById("bonus-box").style.display == "none"){
                    await resetChecker(values.id);
               }
    }

    async function deleteDay(id, num){

        let request = {habitId: id, dayNumber: num};
        let response = await instantHabitApi.days.deleteDay(request);

        if(response.succeeded == true){
            await displayDays();
            document.getElementById("daily-notes").style.display = "none";
        }
        else {
            console.log(response);
        }
    }

    async function addDayDescription(){

        let dayNum = document.getElementById("active-day-id").value;
        let dayDescription = document.getElementById("note-input").value;
        let id = document.getElementById("opened-habit-id").value;
        let request = {habitId: id, dayNumber: dayNum, description: dayDescription};

        let response = await instantHabitApi.days.addDayDescription(request);

        if(response.succeeded == true){
            updateDescription(dayNum);
        }
        else {
            console.log(response);
        }
    }

    function updateDescription(dayNum){

        document.getElementById("note-input").value = "";
        alert(`Description for day ${dayNum} UPDATED.`)
        document.getElementById("daily-notes").style.display = "none";
    }

 
    function dayDescriptionAlert(){

        let confirmAction = confirm("Do you want to add a description");
        
        if(confirmAction){
            document.getElementById("daily-notes").style.display = "block";
        } else {
            document.getElementById("daily-notes").style.display = "none";
        }
    }
 

    async function displayDayDescription(){
        
        let habitId = document.getElementById("opened-habit-id").value;
        let dayNumber = prompt("Enter a day", "0->30/60");
        parseInt(dayNumber);

        let result = await instantHabitApi.days.displayDayDescription(dayNumber,habitId);

            if(result.day == null){
                alert("This day is NOT checked.");
            } else if(result.day.note == null){
                alert("No description found.");
            } else {
                document.getElementById("daily-description-text").innerHTML = result.day.note;
                document.getElementById("daily-description-text").style.display = "block";
        }
    }

    async function getStreak(){

        let habitId = document.getElementById("opened-habit-id").value;
        let response = await instantHabitApi.days.getStreak(habitId);

        document.getElementById("streak-number").innerHTML = `Your best streak: ${response.bestStreak} day/s`;
        document.getElementById("streak-msg").innerHTML = response.motivationalMessage;
    }

    async function resetChecker(dayNumber){

        let habitId = document.getElementById("opened-habit-id").value;
        let result = await instantHabitApi.days.resetChecker(dayNumber, habitId);

        if(result == "You failed."){
            await resetGrid();

        } else if(result == "You succeeded."){
            await getConfirmation();
        }
    }

    async function getConfirmation(){
        alert("You've achieved your goal.You can extend your habit tracking with 30 bonus days or leave your progress as it is.");
            let confirmAction = confirm("Do you want to add 30 bonus days?"); 
                if(confirmAction){
                    await setIsExtended();
                }
    }

    async function resetGrid(){
        alert("You failed to achieve your goal. Your progress will be deleted, but you can always start over.");
            await deleteHabitDays();
            refreshDays();
            await getStreak();
    }

    async function deleteHabitDays(){

        let id = document.getElementById("opened-habit-id").value;
        let request = {habitId: id};

        let response = await instantHabitApi.days.deleteHabitDays(request);

        if(response.succeeded == true){
            await displayDays();
        }
        else {
            console.log(response);
        }
    }

    function refreshDays(){

        for(let i = 1; i <= 60; i++ ){
            let refresh =  document.getElementById(`day-${i}`);
            refresh.style.color = "white";
        }
    }

    async function setIsExtended(){
        let id = document.getElementById("opened-habit-id").value;
        let request = {habitId: id};

        let response = await instantHabitApi.habits.extendHabit(request);

        if(response.succeeded == true){
            document.getElementById("bonus-box").style.display = "block";
        }
        else {
            console.log(response);
        }
    }

    async function handelIsExtended(){

        let habit = await getInfoFromDB(getHabitById());

        if(habit.isExtended == true){
            document.getElementById("bonus-box").style.display = "block";
        } else {
            document.getElementById("bonus-box").style.display = "none";
        }
    }

    
    async function checkForHabitDuplicates(response){
        if(response.succeeded == true){
            await displayHabits();
        } else if (response.succeeded == false && response.error == "Match"){
            alert("This habit already exist.")
        }
         else {
            console.log(response);
        }
        document.getElementById("habit-area").style.display = "none";
        document.getElementById("new-habit-input").value = "";
    }
    

    async function getInfoFromDB(method){

        let task;
        try {
            task = await method;
        } catch (e) {
            console.log("Error!");
            console.log(e);
        }

        return method;
    }

      let instantHabitApi = {
        habits: { 
            extendHabit: async function (apiRequest) {
                let request = apiRequest;
                let result = await fetch(`https://localhost:7181/Habits/ExtendHabit`, {
                method: "PUT",
                mode: 'cors',
                headers: {
                    'Content-Type': 'application/json',
                    'Access-Control-Allow-Origin': '*'
                },
                body: JSON.stringify(request),
            });
            let parsedResponse = await result.json();
            return parsedResponse;
            },
            addHabit: async function (apiRequest){
                let request = apiRequest;
                let result = await fetch(`https://localhost:7181/Habits/AddHabit`, {
                    method: "POST",
                    mode: 'cors',
                    headers: {
                        'Content-Type': 'application/json',
                        'Access-Control-Allow-Origin': '*'
                    },
                    body: JSON.stringify(request),
                });
                let parsedResponse = await result.json();
                return parsedResponse;
            },
            getAllHabits: async function(){
                let response =  await fetch(`https://localhost:7181/Habits/GetAllHabits`, {
                method: "GET",
                mode: 'cors',
                headers: {
                    'Content-Type': 'application/json',
                    'Access-Control-Allow-Origin': '*'
                }
            });
               let result = await response.json();
               return result;
            },
            deleteSQLHabit: async function(apiRequest){
                let request = apiRequest;
                let result =  await fetch(`https://localhost:7181/Habits/DeleteAhabit`, {
                    method: "DELETE",
                    mode: 'cors',
                    headers: {
                        'Content-Type': 'application/json',
                        'Access-Control-Allow-Origin': '*'
                    },
                    body: JSON.stringify(request),
                });
                let parsedResponse = await result.json();
                return parsedResponse;
            },
            addDescription: async function(apiRequest){
                let request = apiRequest;
                let result = await fetch(`https://localhost:7181/Habits/AddDescription`, {
                    method: "PUT",
                    mode: 'cors',
                    headers: {
                        'Content-Type': 'application/json',
                        'Access-Control-Allow-Origin': '*'
                    },
                    body: JSON.stringify(request),
                });
                let parsedResponse = await result.json();
                return parsedResponse;
            },
            getHabitById: async function(habitId){
                let response =  await fetch(`https://localhost:7181/Habits/GetHabitById?id=${habitId}`, {
                    method: "GET",
                    mode: 'cors',
                    headers: {
                        'Content-Type': 'application/json',
                        'Access-Control-Allow-Origin': '*'
                    }
                });
               let result = await response.json();
               return result;
            }
        },
        days: {
            deleteHabitDays: async function (apiRequest) {
                let request = apiRequest;
                let result = await fetch(`https://localhost:7181/Days/DeleteHabitDays`, {
                    method: "DELETE",
                    mode: 'cors',
                    headers: {
                        'Content-Type': 'application/json',
                        'Access-Control-Allow-Origin': '*'
                    },
                    body: JSON.stringify(request),
                });
                let parsedResponse = await result.json();
                return parsedResponse;
           },
           resetChecker: async function(num,id){
                const response =  await fetch(`https://localhost:7181/Days/ResetChecker?dayNumber=${num}&habitId=${id}`, {
                    method: "GET",
                    mode: 'cors',
                    headers: {
                        'Content-Type': 'application/json',
                        'Access-Control-Allow-Origin': '*'
                    }
                });
                const result = await response.text();
                return result;
           },
           getStreak: async function(id){
                const response =  await fetch(`https://localhost:7181/Days/GetBestStreak?habitId=${id}`, {
                    method: "GET",
                    mode: 'cors',
                    headers: {
                        'Content-Type': 'application/json',
                        'Access-Control-Allow-Origin': '*'
                    }
                });
                const result = await response.json();
                return result;
           },
           displayDayDescription: async function(num,id){
                const response =  await fetch(`https://localhost:7181/Days/GetDayByNumber?dayNumber=${num}&habitId=${id}`, {
                    method: "GET",
                    mode: 'cors',
                    headers: {
                        'Content-Type': 'application/json',
                        'Access-Control-Allow-Origin': '*'
                    }
                });

                /*
                if(response.status == 204){
                    alert("The selected day is not checked.");
                } else { */
                    const result = await response.json();
                    return result;
                //}
            },
            addDayDescription: async function(apiRequest){
                let request = apiRequest;
                let result = await fetch(`https://localhost:7181/Days/AddDayDescription`, {
                    method: "PUT",
                    mode: 'cors',
                    headers: {
                        'Content-Type': 'application/json',
                        'Access-Control-Allow-Origin': '*'
                    },
                    body: JSON.stringify(request),
                });
                let parsedResponse = await result.json();
                return parsedResponse;
            },
            deleteDay: async function(apiRequest){
                let request = apiRequest;
                let result = await fetch(`https://localhost:7181/Days/DeleteDay`, {
                    method: "DELETE",
                    mode: 'cors',
                    headers: {
                        'Content-Type': 'application/json',
                        'Access-Control-Allow-Origin': '*'
                    },
                    body: JSON.stringify(request),
                });
                let parsedResponse = await result.json();
                return parsedResponse;
            },
            addDay: async function(apiRequest){
                let request = apiRequest;
                let result = await fetch(`https://localhost:7181/Days/AddDay`, {
                    method: "POST",
                    mode: 'cors',
                    headers: {
                        'Content-Type': 'application/json',
                        'Access-Control-Allow-Origin': '*'
                    },
                    body: JSON.stringify(request),
                });
                let parsedResponse = await result.json();
                return parsedResponse;
            },
            getAllHabitDays: async function(id){
                console.log(id);
                const response =  await fetch(`https://localhost:7181/Days/GetAllHabitDays?habitId=${id}`, {
                    method: "GET",
                    mode: 'cors',
                    headers: {
                        'Content-Type': 'application/json',
                        'Access-Control-Allow-Origin': '*'
                    }
                });
                console.log(response);
                const result = await response.json();
                console.log(result);
                return result;
            }
      }
    }