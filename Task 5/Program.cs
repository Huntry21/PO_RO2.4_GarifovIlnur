using System;
using System.Collections.Generic;
using System.Linq;

public class Student
{
    private static int _idCounter = 1;
    private double _gpa;

    public string Name { get; set; }
    public int StudentId { get; }
    public string Faculty { get; set; }

    public double GPA
    {
        get => _gpa;
        set
        {
            if (value >= 0.0 && value <= 4.0)
            {
                _gpa = value;
            }
        }
    }

    public Student(string name, string faculty, double gpa)
    {
        StudentId = _idCounter++;
        Name = name;
        Faculty = faculty;
        GPA = gpa;
    }

    public override string ToString()
    {
        return $"ID: {StudentId} | Name: {Name} | Faculty: {Faculty} | GPA: {GPA:F2}";
    }
}

public class Registry
{
    private readonly Student[] _students = new Student[100];
    private int _count = 0;

    public bool Add(Student student)
    {
        if (_count < 100)
        {
            _students[_count++] = student;
            return true;
        }
        return false;
    }

    public Student FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_students[i].StudentId == id)
            {
                return _students[i];
            }
        }
        return null;
    }

    public List<Student> FindByName(string name)
    {
        List<Student> results = new List<Student>();
        for (int i = 0; i < _count; i++)
        {
            if (_students[i].Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                results.Add(_students[i]);
            }
        }
        return results;
    }

    public List<Student> GetTopStudents(int n)
    {
        return _students
            .Take(_count)
            .OrderByDescending(s => s.GPA)
            .Take(n)
            .ToList();
    }

    public void PrintAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("The registry is empty.");
            return;
        }

        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_students[i].ToString());
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Registry registry = new Registry();
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n--- Student Registry Menu ---");
            Console.WriteLine("1. Add a new student");
            Console.WriteLine("2. Find a student by ID");
            Console.WriteLine("3. Find students by name");
            Console.WriteLine("4. Display top N students by GPA");
            Console.WriteLine("5. Print all students");
            Console.WriteLine("6. Exit");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter Name: ");
                    string name = Console.ReadLine();
                    Console.Write("Enter Faculty: ");
                    string faculty = Console.ReadLine();
                    Console.Write("Enter GPA (0.0 - 4.0): ");
                    if (double.TryParse(Console.ReadLine(), out double gpa))
                    {
                        if (gpa < 0.0 || gpa > 4.0)
                        {
                            Console.WriteLine("Error: Invalid GPA range.");
                        }
                        else
                        {
                            Student newStudent = new Student(name, faculty, gpa);
                            if (registry.Add(newStudent))
                                Console.WriteLine($"Student added successfully. ID assigned: {newStudent.StudentId}");
                            else
                                Console.WriteLine("Error: Registry is full.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid GPA format.");
                    }
                    break;

                case "2":
                    Console.Write("Enter Student ID: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        Student s = registry.FindById(id);
                        Console.WriteLine(s != null ? s.ToString() : "Student not found.");
                    }
                    break;

                case "3":
                    Console.Write("Enter Name: ");
                    string searchName = Console.ReadLine();
                    var foundByName = registry.FindByName(searchName);
                    if (foundByName.Count > 0)
                        foundByName.ForEach(s => Console.WriteLine(s.ToString()));
                    else
                        Console.WriteLine("No students found with that name.");
                    break;

                case "4":
                    Console.Write("Enter N: ");
                    if (int.TryParse(Console.ReadLine(), out int n))
                    {
                        var topStudents = registry.GetTopStudents(n);
                        topStudents.ForEach(s => Console.WriteLine(s.ToString()));
                    }
                    break;

                case "5":
                    registry.PrintAll();
                    break;

                case "6":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}