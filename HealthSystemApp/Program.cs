public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }

    public Patient(int id, string name, int age, string gender)
    {
        Id = id;
        Name = name;
        Age = age;
        Gender = gender;
    }
}

public class Prescription
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string MedicationName { get; set; }
    public DateTime DateIssued { get; set; }

    public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
    {
        Id = id;
        PatientId = patientId;
        MedicationName = medicationName;
        DateIssued = dateIssued;
    }
}
public class HealthSystemApp
{
    private Repository<Patient> _patientRepo = new Repository<Patient>();
    private Repository<Prescription> _prescriptionRepo = new Repository<Prescription>();
    private Dictionary<int, List<Prescription>> _prescriptionMap = new Dictionary<int, List<Prescription>>();

    public void SeedData()
    {
        // Add sample patients
        _patientRepo.Add(new Patient(1, "John Doe", 30, "Male"));
        _patientRepo.Add(new Patient(2, "Jane Smith", 25, "Female"));
        _patientRepo.Add(new Patient(3, "Samuel Brown", 40, "Male"));

        // Add sample prescriptions
        _prescriptionRepo.Add(new Prescription(1, 1, "Paracetamol", DateTime.Now.AddDays(-2)));
        _prescriptionRepo.Add(new Prescription(2, 1, "Ibuprofen", DateTime.Now.AddDays(-1)));
        _prescriptionRepo.Add(new Prescription(3, 2, "Amoxicillin", DateTime.Now.AddDays(-3)));
        _prescriptionRepo.Add(new Prescription(4, 3, "Cough Syrup", DateTime.Now));
        _prescriptionRepo.Add(new Prescription(5, 2, "Vitamin C", DateTime.Now.AddDays(-5)));
    }

    public void BuildPrescriptionMap()
    {
        foreach (var prescription in _prescriptionRepo.GetAll())
        {
            if (!_prescriptionMap.ContainsKey(prescription.PatientId))
            {
                _prescriptionMap[prescription.PatientId] = new List<Prescription>();
            }
            _prescriptionMap[prescription.PatientId].Add(prescription);
        }
    }

    public void PrintAllPatients()
    {
        Console.WriteLine("\n--- All Patients ---");
        foreach (var patient in _patientRepo.GetAll())
        {
            Console.WriteLine($"[{patient.Id}] {patient.Name} - Age: {patient.Age}, Gender: {patient.Gender}");
        }
    }

    public void PrintPrescriptionsForPatient(int patientId)
    {
        Console.WriteLine($"\n--- Prescriptions for Patient ID: {patientId} ---");
        if (_prescriptionMap.ContainsKey(patientId))
        {
            foreach (var prescription in _prescriptionMap[patientId])
            {
                Console.WriteLine($"[{prescription.Id}] {prescription.MedicationName} - Issued: {prescription.DateIssued}");
            }
        }
        else
        {
            Console.WriteLine("No prescriptions found for this patient.");
        }
    }
}
