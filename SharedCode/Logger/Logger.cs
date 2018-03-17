using System.IO;

public class Logger: BaseLog
    {
    private string filePath = @"C:\Users\Maksim\Desktop\log.txt";

    public override void Log(string message)
    {
        using (StreamWriter streamWriter = new StreamWriter(filePath, true))
        {
            streamWriter.WriteLine(message);
            streamWriter.Close();
        }
    }
}