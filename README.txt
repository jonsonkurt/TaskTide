Task Management Application

Overview
	TaskRide is a web application developed using ASP.NET MVC and C#. It allows users to manage tasks by creating, viewing, editing, and deleting them. The application is divided into three main sections: Dashboard, Ongoing Tasks, and Task Archive.

![alt text](https://github.com/jonsonkurt/TaskTide/blob/master/Preview/Dashboard.png?raw=true)

Features
	Dashboard:
	-Displays the current date.
	-Quick add button for new tasks.
	-Displays statistics: total tasks, completed tasks, overdue tasks, tasks due soon.
	-Shows upcoming tasks within the next three days.

	Ongoing Tasks:
	-Add new tasks with details such as title, description, due date, status, and importance.
	-View a list of all tasks, sortable and filterable by status and importance.
	-Edit task details and delete tasks.
	-Search functionality for quick access to specific tasks.

	Task Archive:
	-Lists all completed tasks.
	-View details of archived tasks.
	-Delete tasks from the archive.

Technologies Used
	-ASP.NET MVC: For building the web application.
	-C#: Backend logic and data manipulation.
	-Entity Framework: For database operations.
	-Bootstrap: For responsive design and styling.
	-HTML/CSS: For structuring and styling the web pages.
	-JavaScript/jQuery: For interactivity and dynamic elements.

Installation and Setup
	Open the Project:
		Open the solution file (.sln) in Visual Studio.
	Configure the Database:
		Update the connection string in web.config to point to your local SQL Server instance.
	Run the following commands in the Package Manager Console to set up the database:
		Update-Database
	Run the Application:
		Press F5 in Visual Studio to build and run the application.

Usage
	Dashboard:
	-View task statistics and upcoming deadlines.
	-Use the quick add button to create new tasks.

	Ongoing Tasks:
	-Click on "Add Task" to create a new task.
	-Use the search bar and filters to find specific tasks.
	-Click on a task to view details, edit, or delete it.

	Task Archive:
	-View completed tasks.
	-Click on a task to view details or delete it.

Future Improvements
	Authentication: Implement user authentication and authorization to manage personal tasks.
	
	Enhanced Responsiveness: Improve responsiveness for a better experience on mobile devices.

	Notifications: Add reminders and notifications for upcoming and overdue tasks.
	
	Sub-tasks: Allow tasks to have sub-tasks for better organization.
