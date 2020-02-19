# VeSync Wrapper For .NET

    VeSync v = new VeSync("username","pass");
    Device[] Devices = v.GetDevices();
    foreach(var d in Devices)
    {
        Console.WriteLine("{0} - {1}", d.Name);
        d.TurnOff();
        d.TurnOn();
    }
