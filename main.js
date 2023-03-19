async function addHabit(){
    let newHabitName = document.getElementById("new-habit-input").value;
    
    let result = await fetch(`https://localhost:7181/Habits/AddHabit?name=${newHabitName}`, {
        method: "POST",
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
    });

    if(result.status == 201){
        await displayHabits();
    }
    else {
        console.log(result.status);
    }

    document.getElementById("new-habit-input").value = "";
}

window.addEventListener('load',displayHabits);

async function GetAllHabits(){
    const response =  await fetch('https://localhost:7181/Habits/GetAllHabits', {
        method: "GET",
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        }
    });
    const result = await response.json();
    return result;
}

async function displayHabits(){
   await GetAllHabits();
    let allHabits = [];

    try {
        allHabits = await GetAllHabits();
    } catch (e) {
        console.log("Error!");
        console.log(e);
    }
    
    let habitsStructure = `
    <div id="habits">
        <h2 id="habits-name-h2">Active habits:</h2>
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

      if(allHabits.length === 0) {
        habitsStructure = "";
        return;
    }

    document.getElementById("habits-area").innerHTML = habitsStructure;

   await setListenersForDeleteBtns();
   await setListenersForOpenBtns();
}

async function openHabit(value){
    
    let nameAndIdArray = value.split("-");
    let name = nameAndIdArray[0];
    let id = nameAndIdArray[1];

    document.getElementById("opened-habit-id").value = id;

    document.getElementById("habit-name-h2").innerHTML = name;
    
    alert(value);
    let habitArea = document.getElementById("habit-area");
    habitArea.style.display = "block";
    await getDescription();
    await displayDays();

}

async function DeleteHabit(e){
    e = e || window.event;
    let allHabits = [];
    let habitArea = document.getElementById("habit-area");
    

    try {
        allHabits = await GetAllHabits();
    } catch (e) {
        console.log("Error!");
        console.log(e);
    }

    for(let i = 0; i < allHabits.length; i++){
        if(allHabits[i].id == e.target.value){
          await deleteSQLHabit(e);
          habitArea.style.display = "none";
          
        }
    }
}

 async function setListenersForOpenBtns(){
    let openBtns = document.getElementsByClassName("open");

    for(let i = 0; i < openBtns.length; i++){
        openBtns[i].addEventListener("click", async function(e){
        await clearDays();
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
    
    let result = await fetch(`https://localhost:7181/Habits/DeleteAhabit?id=${habitToDelete}`, {
        method: "DELETE",
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
    });

    if(result.status == 201){
        await displayHabits();
    }
    else {
        console.log(result.status);
    }

    document.getElementById("new-habit-input").value = "";
}

async function addDescription() {

    let newDescription = document.getElementById("description-input").value;

    let id = document.getElementById("opened-habit-id").value;

    let result = await fetch(`https://localhost:7181/Habits/AddDescription?id=${id}&description=${newDescription}`, {
        method: "PUT",
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
    });

    if(result.status == 201){
        await displayHabits();
    }
    else {
        console.log(result.status);
    }

    document.getElementById("description-input").value = "";
    await getDescription();

}

async function getHabitById(){
    let habitId = document.getElementById("opened-habit-id").value;

    const response =  await fetch(`https://localhost:7181/Habits/GetHabitById?id=${habitId}`, {
        method: "GET",
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        }
    });
    const result = await response.json();
    console.log(result);
    return result;
}

async function getDescription(){
    let selectedHabit;

    try {
         selectedHabit = await getHabitById();
    } catch (e) {
        console.log("Error!");
        console.log(e);
    }

    console.log(selectedHabit.description);

    document.getElementById("description-text").innerHTML = selectedHabit.description;

}

 async function getAllHabitDays(){
    let habitId = document.getElementById("opened-habit-id").value;

    const response =  await fetch(`https://localhost:7181/Days/GetAllHabitDays?habitId=${habitId}`, {
        method: "GET",
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        }
    });
    const result = await response.json();
    console.log(result);
    return result;

 }

 async function displayDays(){
    let days = [];

    try {
         days = await getAllHabitDays();
    } catch (e) {
        console.log("Error!");
        console.log(e);
    }

    console.log(days);

    for (let i = 0; i < days.length; i++){
        let probe = document.getElementById(`day-${days[i].dayNumber}`);
        probe.style.color = "red";
    }
    
 }

  async function clearDays(){
    let days = [];

    try {
         days = await getAllHabitDays();
    } catch (e) {
        console.log("Error!");
        console.log(e);
    }

    console.log(days);

    for (let i = 0; i < days.length; i++){
        let probe = document.getElementById(`day-${days[i].dayNumber}`);
        probe.style.color = "white";
    }
 }

 async function addDay(id){

    let habitId = document.getElementById("opened-habit-id").value;
    let selectedBtn = document.getElementById(id);
    let nameAndIdArray = id.split("-");
    let dayId = nameAndIdArray[1];

    if (selectedBtn.dataset.checker == "true"){
        await deleteDay(habitId,dayId);
        selectedBtn.dataset.checker = false;
        alert("Day DELETED",selectedBtn.dataset.checker);
        selectedBtn.style.color = "white";

    }
    else if (selectedBtn.dataset.checker == "false") {
        let result = await fetch(`https://localhost:7181/Days/AddDay?habitId=${habitId}&dayNumber=${dayId}`, {
        method: "POST",
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        });

       console.log(result);

       if(result.status == 201){
        selectedBtn.dataset.checker = true;
        alert("Day ADDED",selectedBtn.dataset.checker);
        await displayDays();
        dayDescriptionAlert();
       }
       else {
        console.log(result.status);
       }

    }

    }


 async function deleteDay(habitId, dayNumber){

    let result = await fetch(`https://localhost:7181/Days/DeleteDay?habitId=${habitId}&dayNumber=${dayNumber}`, {
        method: "DELETE",
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
    });

    if(result.status == 201){
        await displayDays();
    }
    else {
        console.log(result.status);
    }

 }

 async function addDayDescription(){

    let dayNumber = prompt("Enter the number of your selected day:", "0->30");
    parseInt(dayNumber);
    console.log(dayNumber);
    

    let dayDescription = document.getElementById("note-input").value;
    let habitId = document.getElementById("opened-habit-id").value;

    let result = await fetch(`https://localhost:7181/Days/AddDayDescription?habitId=${habitId}&dayNumber=${dayNumber}&description=${dayDescription}`, {
        method: "PUT",
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
    });

    if(result.status == 201){
        document.getElementById("note-input").value = "";
        document.getElementById("daily-note-text").innerHTML = dayDescription;
    }
    else {
        console.log(result.status);
    }

 }

 
 function dayDescriptionAlert(){
    let confirmAction = confirm("Do you want to add a description");
    let dailyNotes = document.getElementById("daily-notes");
    

    if(confirmAction){
        dailyNotes.style.display = "block";
    }
 }
 

 
 async function displayDayDescription(){

    let dayNumber = prompt("Enter a day", "0->30");
    parseInt(dayNumber);
    console.log(dayNumber);

    let habitId = document.getElementById("opened-habit-id").value;

    const response =  await fetch(`https://localhost:7181/Days/GetDayByNumber?dayNumber=${dayNumber}&habitId=${habitId}`, {
        method: "GET",
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        }
    });
    const result = await response.json();
    console.log(result.note);

    document.getElementById("daily-description-text").innerHTML = result.note;

   // return result;
        
        
    
}

    



