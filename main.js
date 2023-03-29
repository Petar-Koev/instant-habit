async function addHabit(){
    let newHabitName = document.getElementById("new-habit-input").value;

    let request = {
        name: newHabitName
    };
    
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
    console.log(parsedResponse);

    if(parsedResponse.succeeded == true){
        await displayHabits();
    }
    else {
        console.log(parsedResponse);
    }

    document.getElementById("new-habit-input").value = "";
    document.getElementById("habit-area").style.display = "none";
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

     let allHabits = await getInfoFromDB(GetAllHabits());
    
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
    console.log(allHabits);
    console.log(allHabits[0]);

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

    console.log(habitToDelete);

    let request = {
        id: habitToDelete
    };
    
    let result = await fetch(`https://localhost:7181/Habits/DeleteAhabit`, {
        method: "DELETE",
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        body: JSON.stringify(request),
    });

    let parsedResponse = await result.json();
    console.log(parsedResponse);

    if(parsedResponse.succeeded == true){
        await displayHabits();

    }
    else {
        console.log(parsedResult);
    }

}

async function addDescription() {

    let newDescription = document.getElementById("description-input").value;
    let id = document.getElementById("opened-habit-id").value;

    let request = {
        habitId: id,
        description: newDescription
    };

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
        console.log("Parsed : ", parsedResponse);

        await displayHabits();

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
    return result;
}

async function getDescription(){
    let selectedHabit = await getInfoFromDB(getHabitById());

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

    let nameAndIdArray = id.split("-");
    let dayId = nameAndIdArray[1];

    let request = {
        habitId:currentHabitId,
        dayNumber:dayId
    };

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
    console.log(parsedResponse);

    if(parsedResponse.succeeded == true){
        alert("Day ADDED");
        document.getElementById("active-day-id").value = dayId;
        await displayDays();
        await getStreak();
        document.getElementById("daily-description-text").style.display = "none";
        dayDescriptionAlert();
           if(document.getElementById("bonus-box").style.display == "none"){
             await resetChecker(id);
           }
       } else if (parsedResponse.succeeded == false){
        await deleteDay(currentHabitId,dayId);
        alert("Day DELETED");
        selectedBtn.style.color = "white";
        await getStreak();
        document.getElementById("daily-description-text").style.display = "none";
       }
    }


 async function deleteDay(id, num){

    let request = {
        habitId:id,
        dayNumber: num
    };

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

    if(parsedResponse.succeeded == true){
        await displayDays();
    }
    else {
        console.log(parsedResponse);
    }
 }

 async function addDayDescription(){

    let dayNum = document.getElementById("active-day-id").value;

    let dayDescription = document.getElementById("note-input").value;
    let id = document.getElementById("opened-habit-id").value;

    let request = {
        habitId: id,
        dayNumber: dayNum,
        description: dayDescription
    };

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

    if(parsedResponse.succeeded == true){
        document.getElementById("note-input").value = "";
        alert(`Description for day ${dayNum} UPDATED.`)
        document.getElementById("daily-notes").style.display = "none";
    }
    else {
        console.log(parsedResponse);
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
    
    let habitId = document.getElementById("opened-habit-id").value;
    
    let dayNumber = prompt("Enter a day", "0->30/60");
    parseInt(dayNumber);
    
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

    const response =  await fetch(`https://localhost:7181/Days/GetBestStreak?habitId=${habitId}`, {
        method: "GET",
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        }
    });
    const result = await response.json();

    document.getElementById("streak-number").innerHTML = `Your best streak: ${result.bestStreak} day/s`;
    document.getElementById("streak-msg").innerHTML = result.motivationalMessage;
}

async function resetChecker(dayNumber){

    let habitId = document.getElementById("opened-habit-id").value;
    let nameAndIdArray = dayNumber.split("-");
    let dayId = nameAndIdArray[1];
    
    const response =  await fetch(`https://localhost:7181/Days/ResetChecker?dayNumber=${dayId}&habitId=${habitId}`, {
        method: "GET",
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        }
    });
    const result = await response.text();
    
    console.log(result);

    if(result == "You failed."){
        alert("You failed to achieve your goal. Your progress will be deleted, but you can always start over.");
        await deleteHabitDays();
        refreshDays();

    } else if(result == "You succeeded."){
        alert("You've achieved your goal. You can extend your habit tracking with 30 bonus days or leave your progress as it is.");
        let confirmAction = confirm("Do you want to add 30 bonus days?"); 

           if(confirmAction){
             await setIsExtended();
           }
    }
}

async function deleteHabitDays(){

    let id = document.getElementById("opened-habit-id").value;

    let request = {
        habitId:id
    };

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

    if(parsedResponse.succeeded == true){
        await displayDays();
    }
    else {
        console.log(parsedResponse);
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

    let request = {
        habitId: id
    };

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

    if(parsedResponse.succeeded == true){
        document.getElementById("bonus-box").style.display = "block";
    }
    else {
        console.log(parsedResponse);
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
