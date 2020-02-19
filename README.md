# VeSync Wrapper For .NET

This is an unoffical wrapper for VeSync smart switches. Created by Marcello Bachechi in about 45 minutes. Code could be cleaner but it works well.

## Example Usage
    VeSync v = new VeSync("username","pass");
    Device[] Devices = v.GetDevices();
    foreach(var d in Devices)
    {
        Console.WriteLine("Device Name = {1}", d.Name);
        
        //Option 1 to control state
        d.TurnOff();
        d.TurnOn();
        
        //Option 2 to control state
        v.turn_on(d);
        v.turn_off(d);
    }


## Login Data
To see the full return of the login you can call the snippet below. This will allow you to see things such as the profile image URL. 

    v.LoginData
    
Login Data contains
- Token
- Account ID
- Nickname
- Avatar Icon
- User Type
- TOS Accepted in Language
- Terms Status 

### Example

    VeSync v = new VeSync("username","pass");    
    
    MessageBox.Show($"Welcome back, {v.LoginData.NickName}");
    
    //Load image from URL to PictureBox
    var request = WebRequest.Create(v.LoginData.AvatarIconURL);
    
    using (var response = request.GetResponse())
    {
        using (var stream = response.GetResponseStream())        
            pictureBox1.Image = Bitmap.FromStream(stream);        
    }

## Device
If you need more information from the device you can.
Device Contains the following properties
- Name
- ID
- Status
- Connection Type
- Connection Status
- Device Type
- Device Model
- Firmware Version
You can also turn on and off the device using the methods `TurnOn` and `TurnOff`
### Example

    VeSync v = new VeSync("username","pass");
    Device[] Devices = v.GetDevices();
    foreach(var d in Devices)
    {
        Console.WriteLine("Device {1} is currently", d.Name, d.Status);                        
        if(d.Status == "off")
            d.TurnOn();                
    }
