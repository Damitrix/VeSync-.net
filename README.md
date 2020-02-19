# VeSync Wrapper For .NET

    VeSync v = new VeSync("username","pass");
    Device[] Devices = v.GetDevices();
    foreach(var d in Devices)
    {
        Console.WriteLine("{0} - {1}", d.Name);
        //Option 1 to control state
        d.TurnOff();
        d.TurnOn();
        
        //Option 2 to control state
        v.turn_on(d);
        v.turn_off(d);
    }

To see the full return of the login you can call the snippet below. This will allow you to see things such as the profile image URL. 

    v.LoginData
    
