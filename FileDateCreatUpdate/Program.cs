const string MessagePathError      = "Папка не найдена";
const string MessagePathInfo       = "Обработка папки:";
const string MessageFileNameError  = "Не правильное имя файла";
const string MessageHello          = "Обновление даты создания в файлах";
const string MessageOptions        = "Параметры";
const string MessageEnd            = "Закончили";


void MessageError(string message)
{
    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.WriteLine(message);
}

void MessageInfo(string message)
{
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine(message);
}

void UpdateDateCreate(string fileNameFull, DateTime newDate)
{
    File.SetCreationTime(fileNameFull, newDate);
}

DateTime? ParserFileNameDate(string fileName)
{
    //20201231_192826
    //TODO 2012-06-04 17.56.31.jpg
    if (fileName.Length < 15) return null;

    var year = fileName.Substring(0, 4);
    var month = fileName.Substring(4, 2);
    var day = fileName.Substring(6, 2);
    var house = fileName.Substring(9, 2);
    var minute = fileName.Substring(11, 2);
    var second = fileName.Substring(13, 2);

    try
    {
        return new DateTime(
            Convert.ToInt32(year),
            Convert.ToInt32(month),
            Convert.ToInt32(day),
            Convert.ToInt32(house),
            Convert.ToInt32(minute),
            Convert.ToInt32(second)
            );
    }
    catch
    {
        return null;
    }
}

void FileUpdate(string patch)
{
    if (!Directory.Exists(patch))
    {
        MessageError($"\t{MessagePathError}: {patch}");
        return;
    }

    MessageInfo($"{MessagePathInfo} {patch}");

    foreach (var filename in Directory.EnumerateFiles(patch))
    {
        var date = ParserFileNameDate(Path.GetFileName(filename));
        if (date == null)
        {
            MessageError($"\t{MessageFileNameError} {filename}");
            continue;
        }
        UpdateDateCreate(filename, date.Value);
    }
}

MessageInfo(MessageHello);

var arguments = Environment.GetCommandLineArgs();

MessageInfo($"{MessageOptions}: {string.Join(", ", arguments)}");

foreach (var filePath in arguments)
{
    FileUpdate(filePath[^1] == '\\' ? filePath :  filePath + '\\');
}

MessageInfo(MessageEnd);