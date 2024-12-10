//FIB bundle --output C:\Users\user1\Desktop\Michal\bundlefile.txt
using System.CommandLine;

//נוסיף לו מיקום ומה יהיה שם הBUNDLE  במארז
var bundleOption = new Option<FileInfo>(new[] { "--output", "-o" }, "File path and name");
// אופציה לבחירת שפות תכנות
var languageOption = new Option<List<string>>(new[] { "--language", "-l" }, "List of programming languages to include (e.g., cs, java, py). Use 'all' to include all code files.")
{
    IsRequired = true
};
var noteOption = new Option<bool>(new[] { "--note", "-n" }, "Include source file path as a comment in the bundle");
// אופציה לסדר המיון
var sortOption = new Option<string>(new[] { "--sort", "-s" }, () => "name", "Sort files by 'name' (default) or 'type' (file extension).");
// אופציה להסרת שורות ריקות
var removeEmptyLinesOption = new Option<bool>(new[] { "--remove-empty-lines", "-r" }, "Remove empty lines from the source code before bundling.");
// אופציה לרישום שם היוצר
var authorOption = new Option<string>(new[] { "--author", "-a" }, "Name of the file's author");



var bundleCommand = new Command("bundle","BundleCommand code files to a single file");
bundleCommand.AddOption(authorOption);
bundleCommand.AddOption(removeEmptyLinesOption);
bundleCommand.AddOption(bundleOption);
bundleCommand.AddOption(languageOption);
bundleCommand.AddOption(noteOption);
bundleCommand.AddOption(sortOption);
/* bundle  מה להריץ כאשר המשתמש כותב */
bundleCommand.SetHandler((output, languages, includeNote, sortOptionValue, removeEmptyLines, author) => {
    try
    {
        //רשימת סיומות מותרות
        var supportedExtensions = new[] { ".cs", ".java", ".py", ".txt", ".js", ".html", ".css" }; // רשימת סיומות מותרות                                                         
        var excludedFolders = new[] { "bin", "debug", "obj", ".git" };    // החרגת תיקיות לא רצויות
        var currentDirectory = Directory.GetCurrentDirectory();//קבלת התיקיה הנוכחית בה הריצו את הפקודה
        var files = Directory.GetFiles(currentDirectory, "*.*", SearchOption.AllDirectories).Where(file =>
        !excludedFolders.Any(folder => file.Contains(Path.DirectorySeparatorChar + folder + Path.DirectorySeparatorChar)) &&
        supportedExtensions.Contains(Path.GetExtension(file).ToLower()))
        .ToArray(); ;//קבלת כל התיקיות ואף את תתי התיקיות
        if (!languages.Contains("all"))
        {
            files = files.Where(file =>
            {
                var extension =/* מחזירה את סיומת הקובץ */Path.GetExtension(file).TrimStart('.').ToLower();/* לכל סיומת מסיר את הנקודה ואחר כך ממיר הכל לאותיות קטנות */
                return languages.Contains(extension);
            }).ToArray();
        }

        // מיון הקבצים לפי הערך שנבחר באופציה
        files = sortOptionValue.ToLower() switch
        {
            "type" => files.OrderBy(file => Path.GetExtension(file).ToLower()).ToArray(),
            _ => files.OrderBy(file => Path.GetFileName(file).ToLower()).ToArray()
        };

        using (var writer = new StreamWriter(output.FullName) )
        {
            if(!string.IsNullOrEmpty(author))
                writer.WriteLine($"// Author: {author}");
            foreach (var file in files)
            {
                if(includeNote)
                {
                    var relativePath = Path.GetRelativePath(currentDirectory, file);
                    writer.WriteLine($"// --- Source: {relativePath} ---");
                }
                // קריאת התוכן ועיבודו
                var content = File.ReadAllLines(file);
                // הסרת שורות ריקות אם האופציה נבחרה
                if (removeEmptyLines)
                {
                    content = content.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
                }
                foreach (var line in content)
                {
                    writer.WriteLine(line);
                }

            }
        }
    }
    catch(DirectoryNotFoundException ex)
    {
        Console.WriteLine("Error: File path is invalid");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }


}, bundleOption, languageOption, noteOption, sortOption, removeEmptyLinesOption, authorOption);

var createRspCommand = new Command("create-rsp", "Create a response file for bundling command");
createRspCommand.SetHandler(() =>
{
    Console.WriteLine("Creating a response file for the bundle command...");

    Console.Write("Enter output file name (e.g., output.bundle): ");
    var output = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(output))
    {
        Console.WriteLine("Error: Output file name cannot be empty.");
        return;
    }

    Console.Write("Enter programming languages (comma separated, e.g., cs, java, py, or 'all'): ");
    var languages = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(languages))
    {
        Console.WriteLine("Error: You must specify at least one programming language or 'all'.");
        return;
    }

    Console.Write("Remove empty lines? (yes/no): ");
    var removeEmptyLines = Console.ReadLine()?.ToLower() == "yes";

    Console.Write("Include source file path as comments? (yes/no): ");
    var includeNote = Console.ReadLine()?.ToLower() == "yes";

    Console.Write("Sort by (name/type): ");
    var sort = Console.ReadLine();
    if (sort != "name" && sort != "type")
    {
        Console.WriteLine("Error: Sort must be either 'name' or 'type'.");
        return;
    }

    Console.Write("Enter author name: ");
    var author = Console.ReadLine();

    // בניית הפקודה המלאה
    var rspContent = $"bundle --output {output} --language {languages} --sort {sort}";

    if (removeEmptyLines)
        rspContent += " --remove-empty-lines";

    if (includeNote)
        rspContent += " --note";

    if (!string.IsNullOrWhiteSpace(author))
        rspContent += $" --author \"{author}\"";

    // יצירת קובץ ה-rsp
    var rspFileName = "bundle-command.rsp";
    File.WriteAllText(rspFileName, rspContent);

    Console.WriteLine($"Response file created: {rspFileName}");
    Console.WriteLine("Run the command using: FIB @bundle-command.rsp");
});
//פקודת השורש
var rootCommand = new RootCommand("Root command for File Bundler CLI");
rootCommand.AddCommand(bundleCommand);
rootCommand.AddCommand(createRspCommand);
rootCommand.InvokeAsync(args);