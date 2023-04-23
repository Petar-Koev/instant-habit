# Instant Habit


## Summary

_InstantHabit_ allows users to add and manage habits, with each habit displaying a 30-day grid to track daily progress. Users can add and modify descriptions for each habit and specific days. The app calculates streaks, resets progress if less than 20 days are checked after the 25th day, and rewards users with an additional 30-day grid for maintaining momentum.

## Sample usage of Instant Habit application

01. On the main page, there is an input field that allows 
    users to _add a habit_.
	
02. Once a habit is added, it will appear on the left side 
    of the page along with two buttons: "Open" and "Delete".

03. When a habit is opened, the user will see a _30-day grid_ 
    displayed on the right-hand side of the page, along with 
	other related features.
	
04. Users can _add a description_ for each habit and modify it at any time.

05. The grid provides the functionality to mark each day as complete or 
    incomplete by checking or unchecking it. This feature enables the 
	user to track their progress for each habit efficiently.

06. If a day is marked as complete, you can also __add a specific description 
    for that day__.

07. To view the description of any day, click on "Show description" 
    and then enter the number of the chosen day.
	
08. The application automatically calculates your _best streak_ of 
    consecutive checked days.

09. Once you check any day after the 25th day (inclusive), the application 
    will automatically calculate the number of days you have checked so far. 
	If the total is less than 20, your progress will be reset, and you will 
	need to start over. However, if the total is 20 or greater, you will be 
	rewarded with the _opportunity to extend_ your habit tracking by an additional 
	30 days on a _second grid_, so you can continue to track your progress and 
	maintain your momentum.


## Initial setup


1. Execute DB script to initialize the database - instantHabit_init.sql
2. Adjust connection string.
3. Run API project.
4. Run the Client application. 
5. Start using InstantHabit.
  
 

## Screenshots

![This is a alt text.](/Screenshots-readme/(1)habit-input-field.jpg)


## Ideas for future development
* Improve the UI/UX to be more user friendly
* Add unit tests for the JS code for the frontend
* Convert the vanilla JS to React

## Technology Stack
* C#
* .NET 6
* xUnit
* Entity Framework Core 7 (database first approach)
* LINQ
* Moq
* SQL
* JavaScript
* HTML5
* CSS3
* Bootstrap 5
