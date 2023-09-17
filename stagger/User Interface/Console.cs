using Stagger.Model;

namespace Stagger.UserInterface
{
    public class Console : IUserInterface
    {
        Type _staggerType;
        IStagger _stagger;
        List<IProcess> _processes = new();

        public Console()
        {
            _stagger = new FirstComeFirstServed(new Queue<IProcess>());
            _staggerType = _stagger.GetType();
        }

        public void Start() 
        {
            if (!this.Welcome()) return;
            if (!this.PromptForStaggerType()) return;
            if (!this.PromptForMain()) return;
        }

        #region Welcome
        private bool Welcome()
        {
            System.Console.Clear();
            System.Console.WriteLine("-------------------");
            System.Console.WriteLine("Welcome to Stagger!");
            System.Console.WriteLine("Goal: to simulate task scheduling algorithms.");
            System.Console.WriteLine();
            System.Console.WriteLine("Choose an option.");
            System.Console.WriteLine("1. [N]ext");
            System.Console.WriteLine("2. [S]top");
            System.Console.Write("option: ");

            return this.ProcessFirstOption();
        }

        private bool ProcessFirstOption()
        {
            string? option = System.Console.ReadLine();
            System.Console.WriteLine();

            switch (option)
            {
                case "Next":
                case "N":
                case "n":
                case "1":
                    return true;
                case "Stop":
                case "S":
                case "s":
                case "2":
                    System.Console.WriteLine("Good bye!");
                    return false;
                default:
                    System.Console.WriteLine("Not a valid option.");
                    System.Console.WriteLine("Good bye!");
                    return false;                    
            }
        }
        #endregion

        #region Stagger
        private bool PromptForStaggerType()
        {
            System.Console.Clear();
            System.Console.WriteLine("-------------------");
            System.Console.WriteLine("Task #1: Choose one from these available algorithms.");
            System.Console.WriteLine();
            System.Console.WriteLine("1. [FF] First Come First Served");
            System.Console.WriteLine("2. [SJ] Shortest Job First");
            System.Console.WriteLine("3. [RR] Round Robin");
            System.Console.WriteLine("4. [SR] Shortest Remaining Time First");
            System.Console.Write("option: ");

            return this.ProcessSecondOption();
        }

        private bool ProcessSecondOption()
        {
            string? option = System.Console.ReadLine();
            System.Console.WriteLine();

            switch (option)
            {
                case "FF":
                case "ff":
                case "Ff":
                case "fF":
                case "1":
                    _staggerType = typeof(FirstComeFirstServed);
                    _processes = new ();
                    return true;
                case "SJ":
                case "sj":
                case "Sj":
                case "sJ":
                case "2":
                    System.Console.Clear();
                    System.Console.WriteLine("You have choosen 2. [SJ] Shortest Job First");
                    System.Console.WriteLine("Unfortunately it has not been implemented yet :/");
                    System.Console.WriteLine("Good bye!");
                    return false;
                case "RR":
                case "rr":
                case "Rr":
                case "rR":
                case "3":
                    System.Console.Clear();
                    System.Console.WriteLine("You have choosen 3. [RR] Round Robin");
                    System.Console.WriteLine("Unfortunately it has not been implemented yet :/");
                    System.Console.WriteLine("Good bye!");
                    return false;
                case "SR":
                case "sr":
                case "Sr":
                case "sR":
                case "4":
                    System.Console.Clear();
                    System.Console.WriteLine("You have choosen 4. [SR] Shortest Remaining Time First");
                    System.Console.WriteLine("Unfortunately it has not been implemented yet :/");
                    System.Console.WriteLine("Good bye!");
                    return false;
                default:
                    System.Console.WriteLine("Not a valid option.");
                    System.Console.WriteLine("Good bye!");
                    return false;
            }
        }
        #endregion

        #region Processes
        private bool PromptForMain()
        {
            do 
            {
                System.Console.Clear();
                System.Console.WriteLine("-------------------");
                System.Console.WriteLine($"You chose {_stagger.Name}.");
                System.Console.WriteLine();

                System.Console.ForegroundColor = ConsoleColor.Blue;                
                System.Console.WriteLine($"Processes");
                System.Console.WriteLine($"Count: {_processes.Count}");
                System.Console.ResetColor();
                
                System.Console.WriteLine();
                System.Console.WriteLine("Task #2: Add more processes.");
                System.Console.WriteLine();
                System.Console.WriteLine("Choose an option.");
                System.Console.WriteLine("1. [A]dd new process");
                System.Console.WriteLine("2. [S]imulate");
                System.Console.WriteLine("3. [Q]uit");
                System.Console.Write("option: ");
            } while (this.ProcessThirdOption());

            return true;
        }

        private bool ProcessThirdOption()
        {
            string? option = System.Console.ReadLine();
            System.Console.WriteLine();

            switch (option)
            {
                case "A":
                case "a":
                case "1":
                    return this.PromptForProcess();
                case "S":
                case "s":
                case "2":
                    return this.PromptSimulation();
                case "Q":
                case "q":
                case "3":
                    return false;
                default:
                    System.Console.WriteLine("Not a valid option.");
                    System.Console.WriteLine("Good bye!");
                    return false;
            }
        }

        private bool PromptForProcess()
        {
            System.Console.Clear();
            System.Console.WriteLine("-------------------");            

            int arrivalTime, priority, steps;

            System.Console.Write("Arrival time: ");
            while (!int.TryParse(System.Console.ReadLine(), out arrivalTime) || arrivalTime < 0 || arrivalTime > 15)
            {
                System.Console.WriteLine("Invalid Arrival Time! Type an integer number between 0 and 15.");
                System.Console.WriteLine();
                System.Console.Write("Arrival time: ");
            }
            System.Console.WriteLine($"Arrival time: {arrivalTime}");
            System.Console.WriteLine();

            System.Console.Write("Priority: ");
            while (!int.TryParse(System.Console.ReadLine(), out priority) || priority < 1 || priority > 5)
            {
                System.Console.WriteLine("Invalid Priority! Type an integer number between 1 and 5.");
                System.Console.WriteLine();
                System.Console.Write("Priority: ");
            }
            System.Console.WriteLine($"Arrival time: {priority}");
            System.Console.WriteLine();

            System.Console.Write("Steps: ");
            while (!int.TryParse(System.Console.ReadLine(), out steps) || steps < 1 || steps > 10)
            {
                System.Console.WriteLine("Invalid Steps! Type an integer number between 1 and 10.");
                System.Console.WriteLine();
                System.Console.Write("Steps: ");
            }
            System.Console.WriteLine($"Arrival time: {steps}");
            System.Console.WriteLine();

            _processes.Add(new Process(arrivalTime, priority, steps));

            return true;
        }
        #endregion

        private bool PromptSimulation()
        {
            this.InstantiateStagger();
            int iteration = 0;
            do 
            {
                iteration++;
                this.PromptStep();
                this.PromptSimulationStatus(iteration);
                if (this.PromptConfirmation()) break;
            } while (_stagger.Busy);

            return PromptStatistics();
        }

        private bool PromptStep()
        {
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.Clear();
            System.Console.WriteLine("-------------------");
            System.Console.WriteLine($"Starting to work on a process..");
            System.Console.Write($"Type any key to continue..");
            System.Console.ReadLine();
            _stagger.Work(System.Console.WriteLine);
            System.Console.WriteLine();
            System.Console.ResetColor();

            return true;
        }

        private void PromptSimulationStatus(int iteration)
        {            
            System.Console.WriteLine($"Iteration {iteration}");
            System.Console.WriteLine();
            
            this.PromptProgress();
            System.Console.WriteLine();

            this.PromptQueue();
        }

        private void PromptProgress()
        {
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine("-------------------");
            System.Console.WriteLine($"Result");

            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.Write(new string('|', _stagger.Current));
            System.Console.ForegroundColor = ConsoleColor.Gray;
            System.Console.Write(new string('|', _stagger.Length - _stagger.Current));
            System.Console.WriteLine();
            System.Console.ResetColor();
        }

        private void PromptQueue()
        {
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine("-------------------");
            System.Console.WriteLine($"Queue");
            foreach (IProcess process in _stagger.Ready)
            {
                System.Console.Write($"Process ID {process.ID.ToString().PadLeft(4, '0')} : ");
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.Write($"{new string('|', process.CurrentStep)}");
                System.Console.ForegroundColor = ConsoleColor.Gray;
                System.Console.WriteLine($"{new string('|', process.Steps - process.CurrentStep)}");
            }
            System.Console.WriteLine();
            System.Console.ResetColor();
        }

        private bool PromptStatistics()
        {
            return true;
        }

        private void InstantiateStagger()
        {
            var constructor = _staggerType.GetConstructor(new Type[1] {typeof(IEnumerable<IProcess>)});
            constructor?.Invoke(_stagger, new object[] { _processes });
        }

        private bool PromptConfirmation()
        {
            System.Console.WriteLine("Do you wish to continue?");
            System.Console.WriteLine("1. [Y]es");
            System.Console.WriteLine("2. [N]o");
            System.Console.Write("option: ");

            return this.ProcessConfirmation();
        }

        private bool ProcessConfirmation()
        {
            string option = System.Console.ReadLine() ?? "N";

            switch (option)
            {
                case "Y":
                case "y":
                case "1":
                    return true;
                case "N":
                case "n":
                case "2":
                    System.Console.WriteLine("Good bye!");
                    return false;
                default:
                    System.Console.WriteLine("Not a valid option.");
                    System.Console.WriteLine("Good bye!");
                    return false;
            }
        }
    }
}