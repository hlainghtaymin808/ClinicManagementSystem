using System;

public class Patient
{
    // Patient properties
    public int PatientID { get; set; } // short version of get {return PatientID;}, set { PatientID = value;}

    public string PatientName { get; set; }
    public int PatientAge { get; set; }
    public string Symptoms { get; set; }

    // NEW: Store each patient's consultation fee
    public double ConsultationFee { get; set; }

    // Constructor
    public Patient(int id, string name, int age, string symptoms)
    {
        PatientID = id;
        PatientName = name;
        PatientAge = age;
        Symptoms = symptoms;

        // NEW: Initially, no consultation fee has been calculated
        ConsultationFee = 0;
    }
}
public class Program
{
    // Existing: One-dimensional array for patient objects
    static Patient[] patients = new Patient[100];

    static int patientCount = 0;

    // NEW: Two-dimensional array for appointment schedules
    // 5 rows = Monday to Friday
    // 4 columns = 4 appointment times per day
    // 0 means the appointment slot is available
    static int[,] appointments = new int[5, 4];

    // NEW: Day names
    static string[] days =
    {
        "Monday",
        "Tuesday",
        "Wednesday",
        "Thursday",
        "Friday"
    };

    // NEW: Appointment times
    static string[] times =
    {
        "09:00",
        "11:00",
        "13:00",
        "15:00"
    };


    public static void Main()
    {
        bool keepRunning = true;

        while (keepRunning)
        {
            Console.WriteLine("\n=======================================");
            Console.WriteLine("        CLINIC MANAGEMENT SYSTEM       ");
            Console.WriteLine("=======================================");
            Console.WriteLine("1. Register New Patient");
            Console.WriteLine("2. View All Patients");
            Console.WriteLine("3. Search Patient");
            Console.WriteLine("4. Calculate Consultation Fee");
            Console.WriteLine("5. Book Appointment");       // NEW
            Console.WriteLine("6. View Appointment Schedule"); // NEW
            Console.WriteLine("7. Exit");                   // MODIFIED
            Console.WriteLine("=======================================");
            Console.Write("Select an option (1-7): ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    RegisterPatient();
                    break;

                case "2":
                    ViewPatients();
                    break;

                case "3":
                    SearchPatient();
                    break;

                case "4":
                    CalculateFee();
                    break;

                case "5":                   // NEW
                    BookAppointment();
                    break;

                case "6":                   // NEW
                    ViewSchedule();
                    break;

                case "7":                   // MODIFIED
                    keepRunning = false;
                    Console.WriteLine("Thank you for using the system!");
                    break;

                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }

            if (keepRunning)
            {
                Console.WriteLine("\nPress Enter to return to main menu...");
                Console.ReadLine();
            }
        }
    }

    // STEP 1: IMPROVED PATIENT REGISTRATION
    public static void RegisterPatient()
    {
        // Check whether the patient array is full
        if (patientCount >= patients.Length)
        {
            Console.WriteLine("Patient storage is full!");
            return;
        }

        Console.WriteLine("\n========== PATIENT REGISTRATION ==========");
        int id;    //change the coding with while loop
        while (true)
        {
            Console.Write("Enter Patient ID: ");

            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Please enter a valid number!");
                continue;
            }

            if (id <= 0)
            {
                Console.WriteLine("Patient ID must be positive!");
                continue;
            }

            if (FindPatient(id) != -1)
            {
                Console.WriteLine("This Patient ID already exists!");
                continue;
            }

            break;
        }

        Console.Write("Enter Patient Name: ");
        string name = Console.ReadLine();

        Console.Write("Enter Patient Age: ");

        int age = int.Parse(Console.ReadLine());

        //// NEW: Prevent negative ages
        if (age < 0 || age > 100)
        {
            Console.WriteLine("Invalid age!");
            return;
        }
        else if (age == 0)
        {
            Console.WriteLine("Patient is a newborn.");
        }
        else
        {
            Console.WriteLine("Patient age accepted.");
        }

        Console.Write("Enter Patient Symptoms: ");
        string symptoms = Console.ReadLine();

        // Create a new Patient object
        Patient newPatient = new Patient(id, name, age, symptoms);

        // Store the object in the patient array
        patients[patientCount] = newPatient;

        // Increase the patient count
        patientCount++;

        Console.WriteLine("\nPatient registered successfully!");
    }


    // STEP 2: IMPROVED PATIENT RECORDS
    public static void ViewPatients()
    {
        Console.WriteLine("\n========================== PATIENT RECORDS ==========================");

        if (patientCount == 0)
        {
            Console.WriteLine("No patients registered yet.");
            return;
        }

        // MODIFIED: Display column headings with proper spacing
        Console.WriteLine(
            "{0,-12} {1,-20} {2,-8} {3,-22} {4,-12}",
            "Patient ID",
            "Name",
            "Age",
            "Symptoms",
            "Fee ($)"
        );

        Console.WriteLine("------------------------------------------------------------------------");

        for (int i = 0; i < patientCount; i++)
        {
            Patient p = patients[i];

            // Display each patient's information
            Console.WriteLine(
                "{0,-12} {1,-20} {2,-8} {3,-22} {4,-12}",
                p.PatientID,
                p.PatientName,
                p.PatientAge,
                p.Symptoms,
                p.ConsultationFee
            );
        }
    }


    // STEP 2: SEARCH PATIENT
    public static void SearchPatient()
    {
        Console.Write("\nEnter Patient ID to search: ");
        int searchID = int.Parse(Console.ReadLine());

        int index = FindPatient(searchID);

        if (index != -1)
        {
            Patient p = patients[index];

            Console.WriteLine("\n========== PATIENT FOUND ==========");
            Console.WriteLine("Patient ID: " + p.PatientID);
            Console.WriteLine("Name: " + p.PatientName);
            Console.WriteLine("Age: " + p.PatientAge);
            Console.WriteLine("Symptoms: " + p.Symptoms);

            // NEW: Display the patient's consultation fee
            Console.WriteLine("Consultation Fee: $" + p.ConsultationFee);
        }
        else
        {
            Console.WriteLine("Patient not found.");
        }
    }


    // EXISTING: FIND PATIENT BY ID
    public static int FindPatient(int searchID)
    {
        for (int i = 0; i < patientCount; i++)
        {
            if (patients[i].PatientID == searchID)
            {
                // Return the array index if found
                return i;
            }
        }

        // Return -1 if the patient does not exist
        return -1;
    }


    // STEP 3: CONNECT CONSULTATION FEES TO PATIENTS
    public static void CalculateFee()
    {
        Console.WriteLine("\n========== CONSULTATION FEE ==========");

        // NEW: Ask which patient is receiving the consultation
        Console.Write("Enter Patient ID: ");
        int searchID = int.Parse(Console.ReadLine());

        int index = FindPatient(searchID);

        // Check whether the patient exists
        if (index == -1)
        {
            Console.WriteLine("Patient not found.");
            return;
        }

        // Get the patient object from the array
        Patient p = patients[index];

        Console.WriteLine("\nPatient: " + p.PatientName);

        Console.WriteLine("\n1. General Consultation - $15");
        Console.WriteLine("2. Specialist Consultation - $30");
        Console.WriteLine("3. Emergency Consultation - $50");

        Console.Write("Select consultation type (1-3): ");

        string choice = Console.ReadLine();

        double fee = 0;

        switch (choice)
        {
            case "1":
                fee = 15;
                break;

            case "2":
                fee = 30;
                break;

            case "3":
                fee = 50;
                break;

            default:
                Console.WriteLine("Invalid consultation type.");
                return;
        }

        // NEW: Save the fee inside the patient's object
        p.ConsultationFee = fee;

        Console.WriteLine("\nConsultation fee calculated successfully!");
        Console.WriteLine("Patient: " + p.PatientName);
        Console.WriteLine("Consultation Fee = $" + p.ConsultationFee);
    }


    // STEP 5: BOOK APPOINTMENT USING THE 2D ARRAY
    public static void BookAppointment()
    {
        Console.WriteLine("\n========== BOOK APPOINTMENT ==========");

        // Ask for patient ID
        Console.Write("Enter Patient ID: ");
        int searchID = int.Parse(Console.ReadLine());

        // Check whether the patient exists
        int patientIndex = FindPatient(searchID);

        if (patientIndex == -1)
        {
            Console.WriteLine("Patient not found. Please register first.");
            return;
        }

        Patient p = patients[patientIndex];

        Console.WriteLine("\nPatient: " + p.PatientName);

        // Display available days
        Console.WriteLine("\nSelect a day:");

        for (int i = 0; i < days.Length; i++)
        {
            Console.WriteLine((i + 1) + ". " + days[i]);
        }

        Console.Write("Enter day (1-5): ");
        int dayChoice = Convert.ToInt32(Console.ReadLine());

        // Check whether the day selection is valid
        if (dayChoice < 1 || dayChoice > 5)
        {
            Console.WriteLine("Invalid day selection.");
            return;
        }

        // Display available appointment times
        Console.WriteLine("\nSelect an appointment time:");

        for (int i = 0; i < times.Length; i++)
        {
            Console.WriteLine((i + 1) + ". " + times[i]);
        }

        Console.Write("Enter time (1-4): ");
        int timeChoice = Convert.ToInt32(Console.ReadLine());

        // Check whether the time selection is valid
        if (timeChoice < 1 || timeChoice > 4)
        {
            Console.WriteLine("Invalid time selection.");
            return;
        }

        // Convert user selections into array indexes
        int dayIndex = dayChoice - 1;
        int timeIndex = timeChoice - 1;

        // Check whether the selected appointment slot is occupied
        if (appointments[dayIndex, timeIndex] != 0)
        {
            Console.WriteLine("This appointment slot is already booked!");
            return;
        }

        // Store the patient's ID in the 2D array
        appointments[dayIndex, timeIndex] = searchID;

        Console.WriteLine("\nAppointment booked successfully!");
        Console.WriteLine("Patient: " + p.PatientName);
        Console.WriteLine("Day: " + days[dayIndex]);
        Console.WriteLine("Time: " + times[timeIndex]);
    }


    // STEP 6: DISPLAY THE APPOINTMENT TIMETABLE
    public static void ViewSchedule()
    {
        Console.WriteLine("\n========================== APPOINTMENT SCHEDULE ==========================");

        // Display the column headings
        Console.Write("{0,-15}", "Day");

        for (int j = 0; j < times.Length; j++)
        {
            Console.Write("{0,-15}", times[j]);
        }

        Console.WriteLine();

        Console.WriteLine("-------------------------------------------------------------------------");

        // Outer loop: move through each day
        for (int i = 0; i < days.Length; i++)
        {
            Console.Write("{0,-15}", days[i]);

            // Inner loop: move through each appointment time
            for (int j = 0; j < times.Length; j++)
            {
                // Check whether the appointment slot is empty
                if (appointments[i, j] == 0)
                {
                    Console.Write("{0,-15}", "----");
                }
                else
                {
                    // Get the patient ID stored in the appointment array
                    int patientID = appointments[i, j];

                    // Find the patient's information
                    int patientIndex = FindPatient(patientID);

                    if (patientIndex != -1)
                    {
                        Console.Write(
                            "{0,-15}",
                            patients[patientIndex].PatientName
                        );
                    }
                }
            }

            // Move to the next row (day)
            Console.WriteLine();
        }
    }
}