// See https://aka.ms/new-console-template for more information
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        // 1. Prompt user for name
        Console.Write("Enter your name: ");
        string name = Console.ReadLine();

        // 2. Prompt user for birthdate and validate format using MM/dd/yyyy
        string birthdate = GetValidatedBirthdate();

        // 3. Calculate and display user age
        DateTime birthDateTime = DateTime.ParseExact(birthdate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        int age = CalculateAge(birthDateTime);
        Console.WriteLine($"Hello {name}, you are {age} years old.");

        // 4. Save user info to file named user_info.txt
        SaveUserInfo(name, birthdate);

        // 5. Prompt user to enter directory path and list all files in the directory
        Console.Write("Enter a directory path to list files: ");
        string directoryPath = Console.ReadLine();
        ListFilesInDirectory(directoryPath);

        // 6. Prompt user to enter a string and format it to title case
        Console.Write("Enter a string to format to Title Case: ");
        string userInput = Console.ReadLine();
        string titleCaseString = ToTitleCase(userInput);
        Console.WriteLine($"Formatted string: {titleCaseString}");

        // 7. Explicitly trigger garbage collection
        Console.WriteLine("Triggering garbage collection...");
        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.WriteLine("Garbage collection complete.");
    }

    static string GetValidatedBirthdate()
    {
        string pattern = @"^(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])/\d{4}$"; // MM/dd/yyyy format
        Regex regex = new Regex(pattern);
        string birthdate;

        while (true)
        {
            Console.Write("Enter your birthdate (MM/dd/yyyy): ");
            birthdate = Console.ReadLine();

            if (regex.IsMatch(birthdate))
            {
                DateTime parsedDate;
                if (DateTime.TryParseExact(birthdate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    break;
                }
            }
            Console.WriteLine("Invalid date format. Please try again.");
        }

        return birthdate;
    }

    static int CalculateAge(DateTime birthDate)
    {
        DateTime today = DateTime.Today;
        int age = today.Year - birthDate.Year;

        // Adjust for the case where the birthday hasn't occurred yet this year
        if (birthDate > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }

    static void SaveUserInfo(string name, string birthdate)
    {
        string filePath = "user_info.txt";
        string userInfo = $"Name: {name}\nBirthdate: {birthdate}";

        File.WriteAllText(filePath, userInfo);
        Console.WriteLine($"User info saved to {filePath}");
    }

    static void ListFilesInDirectory(string directoryPath)
    {
        try
        {
            if (Directory.Exists(directoryPath))
            {
                string[] files = Directory.GetFiles(directoryPath);
                Console.WriteLine("Files in directory:");

                foreach (string file in files)
                {
                    Console.WriteLine(file);
                }
            }
            else
            {
                Console.WriteLine("Directory does not exist.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error listing files: {ex.Message}");
        }
    }

    static string ToTitleCase(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(input.ToLower());
    }
}
