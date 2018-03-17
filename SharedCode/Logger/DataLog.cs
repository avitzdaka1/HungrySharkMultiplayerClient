using System.IO;

public class DataLog : BaseLog
{
    private string filePath = @"C:\Users\Maksim\Desktop\data.txt";

    public override void Log(string message)
    {
        using (StreamWriter streamWriter = new StreamWriter(filePath, true))
        {
            streamWriter.WriteLine(message);
            streamWriter.Close();
        }
    }
}