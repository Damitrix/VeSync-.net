# VeSync Wrapper For .NET

    VeSync v = new VeSync("username","pass");
    Device[] Devices = v.GetDevices();
    foreach(var d in Devices)
    {
        Console.WriteLine("{0} - {1}", d.Name);
        d.TurnOff();
        d.TurnOn();
    }

To see the full return of the login you can call the snippet below. This will allow you to see things such as the profile image URL. 

    v.LoginData
    
