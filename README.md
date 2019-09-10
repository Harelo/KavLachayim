<h1><b>KavLachayim's mobile app</h1></b>

KavLachayim is a project I made for the organization for which I've done my national service.<br/>
This project was written in C# using the .NET framework (Specifically Xamarin.Forms) with the MVVM architectual-pattern in mind, and is currently only working for Android.<br/>
The project is KavLachyim's <a href="https://www.kavlachayim.co.il">website</a> in native mobile app form, and with it you can view and donate money to KavLachayim's projects, as well as read other information about the organization.<br/><br/>

The app is a database-based app, meaning that the information within it is dynamic (taken from the database) and can be easily changed by changing the data in the database.<br/> For that, there is an external RESTful API made with .NET.CORE (this project's repository can be found <a href="https://www.github.com/Harelo/KavLachayimAPI">here</a>.), which is used to automatically update the information stored locally on the user's device.
