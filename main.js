    async function addHabit(){
        let newHabitName = document.getElementById("new-habit-input").value;
        let request = {name: newHabitName};

        let response = await dbFetch("Habits","AddHabit","POST", request);
        
        if(response.succeeded == true){
            await displayHabits();
        } else {
            console.log(response);
        }

        document.getElementById("new-habit-input").value = "";
        document.getElementById("habit-area").style.display = "none";
    }

    window.addEventListener('load',displayHabits);

    async function GetAllHabits(){

        let url = `Habits/GetAllHabits`;
        let response = await queryFetch(url, "GET");

        return response;
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
    }

    async function setListenersForOpenBtns(){
        let openBtns = document.getElementsByClassName("open");

        for(let i = 0; i < openBtns.length; i++){
            openBtns[i].addEventListener("click", async function(e){
            await displayDays();
            await openHabit(openBtns[i].value);
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

        let response = await dbFetch("Habits","DeleteAhabit","DELETE",request);

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

            let response = await dbFetch("Habits","AddDescription","PUT",request);
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

        let url = `Habits/GetHabitById?id=${habitId}`;
        let response = await queryFetch(url, "GET");

        return response;
    }

    async function getDescription(){

        let selectedHabit = await getInfoFromDB(getHabitById());

        document.getElementById("description-text").innerHTML = selectedHabit.description;
    }

    async function getAllHabitDays(){

        let habitId = document.getElementById("opened-habit-id").value;

        let url = `Days/GetAllHabitDays?habitId=${habitId}`;
        let response = await queryFetch(url, "GET");

        return  response;
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

        let response = await dbFetch("Days", "AddDay", "POST", request);

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
        let response = await dbFetch("Days","DeleteDay","DELETE",request);

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

        let response = await dbFetch("Days", "AddDayDescription", "PUT", request);

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
        }
    }
 

    async function displayDayDescription(){
        
        let habitId = document.getElementById("opened-habit-id").value;
        let dayNumber = prompt("Enter a day", "0->30/60");
        parseInt(dayNumber);

        /*
        let url = `Days/GetDayByNumber?dayNumber=${dayNumber}&habitId=${habitId}`;

        let response = await queryFetch(url, "GET");
        */
        
        
        const response =  await fetch(`https://localhost:7181/Days/GetDayByNumber?dayNumber=${dayNumber}&habitId=${habitId}`, {
            method: "GET",
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json',
                'Access-Control-Allow-Origin': '*'
            }
        });

        if(response.status == 204){
            alert("The selected day is not checked.");
        } else {
            const result = await response.json();
            if(result.note == null){
                alert("No description found.")
            } else {
                document.getElementById("daily-description-text").innerHTML = result.note;
                document.getElementById("daily-description-text").style.display = "block";
            }
        }
    }

    async function getStreak(){

        let habitId = document.getElementById("opened-habit-id").value;
        let url = `Days/GetBestStreak?habitId=${habitId}`;

        let response = await queryFetch(url, "GET");

        document.getElementById("streak-number").innerHTML = `Your best streak: ${response.bestStreak} day/s`;
        document.getElementById("streak-msg").innerHTML = response.motivationalMessage;
    }

    async function resetChecker(dayNumber){

        let habitId = document.getElementById("opened-habit-id").value;

        const response =  await fetch(`https://localhost:7181/Days/ResetChecker?dayNumber=${dayNumber}&habitId=${habitId}`, {
            method: "GET",
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json',
                'Access-Control-Allow-Origin': '*'
            }
        });
        const result = await response.text();

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

        let response = await dbFetch("Days", "DeleteHabitDays", "DELETE", request);

        if(response.succeeded == true){
            await displayDays();
        }
        else {
            console.log(response);
        }
    }

    function refreshDays(){

        for(let i = 1; i <= 30; i++ ){
            let refresh =  document.getElementById(`day-${i}`);
            refresh.style.color = "white";
        }
    }

    async function setIsExtended(){
        let id = document.getElementById("opened-habit-id").value;
        let request = {habitId: id};

        let response = await dbFetch("Habits","ExtendHabit","PUT",request);

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

    async function dbFetch(controller,apiMethod,operation,apiRequest){
        let request = apiRequest;

        let result = await fetch(`https://localhost:7181/${controller}/${apiMethod}`, {
            method: operation,
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json',
                'Access-Control-Allow-Origin': '*'
            },
            body: JSON.stringify(request),
        });

        let parsedResponse = await result.json();
        return parsedResponse;
    }

    async function queryFetch(url, apiMethod) {
       const response =  await fetch(`https://localhost:7181/${url}`, {
            method: apiMethod,
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json',
                'Access-Control-Allow-Origin': '*'
            }
        });
        const result = await response.json();
        return result;
      }

      // TODO: 
      /*
      let InstantHabitApi = {
        getDaysByNumber: async function () {
            const response =  await fetch(`https://localhost:7181/Days/GetDayByNumber?dayNumber=${dayNumber}&habitId=${habitId}`, {
                method: "GET",
                mode: 'cors',
                headers: {
                    'Content-Type': 'application/json',
                    'Access-Control-Allow-Origin': '*'
                }
            });
            
            return response;
        },
        
        getBestStreak: function () {
        
        }
        
    }
    */
