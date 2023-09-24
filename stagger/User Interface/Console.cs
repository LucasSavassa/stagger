using Stagger.Model;

namespace Stagger.UserInterface
{
    public class ConsoleUI : IUserInterface
    {
        IStagger? _stagger;

        public ConsoleUI()
        { }

        public void Start() 
        {
            if (!this.Welcome()) return;
            while(true)
            {
                if (!this.PromptForStaggerType()) break;
                this.PromptForMain();
            }
        }

        #region Welcome
        private bool Welcome()
        {
            Console.Clear();
            Console.WriteLine("-------------------");
            Console.WriteLine("Welcome to Stagger!");
            Console.WriteLine("Goal: to simulate task scheduling algorithms.");
            Console.WriteLine();
            Console.WriteLine("Choose an option.");
            Console.WriteLine("1. [N]ext");
            Console.WriteLine("2. [S]top");
            Console.Write("option: ");

            return this.ProcessFirstOption();
        }

        private bool ProcessFirstOption()
        {
            string? option = Console.ReadLine();
            Console.WriteLine();

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
                    Console.WriteLine("Good bye!");
                    return false;
                default:
                    Console.WriteLine("Not a valid option.");
                    Console.WriteLine("Good bye!");
                    return false;                    
            }
        }
        #endregion

        #region Stagger
        private bool PromptForStaggerType()
        {
            Console.Clear();
            Console.WriteLine("-------------------");
            Console.WriteLine("Task #1: Choose one from these available algorithms.");
            Console.WriteLine();
            Console.WriteLine("1. [FF] First Come First Served");
            Console.WriteLine("2. [SJ] Shortest Job First");
            Console.WriteLine("3. [RR] Round Robin");
            Console.WriteLine("4. [SR] Shortest Remaining Time First");
            Console.Write("option: ");

            return this.ProcessSecondOption();
        }

        private bool ProcessSecondOption()
        {
            string? option = Console.ReadLine();
            Console.WriteLine();

            switch (option)
            {
                case "FF":
                case "ff":
                case "Ff":
                case "fF":
                case "1":
                    _stagger = new FirstComeFirstServed();
                    return true;
                case "SJ":
                case "sj":
                case "Sj":
                case "sJ":
                case "2":
                    Console.Clear();
                    Console.WriteLine("You have choosen 2. [SJ] Shortest Job First");
                    Console.WriteLine("Unfortunately it has not been implemented yet :/");
                    Console.WriteLine("Good bye!");
                    return false;
                case "RR":
                case "rr":
                case "Rr":
                case "rR":
                case "3":
                    Console.Clear();
                    Console.WriteLine("You have choosen 3. [RR] Round Robin");
                    Console.WriteLine("Unfortunately it has not been implemented yet :/");
                    Console.WriteLine("Good bye!");
                    return false;
                case "SR":
                case "sr":
                case "Sr":
                case "sR":
                case "4":
                    Console.Clear();
                    Console.WriteLine("You have choosen 4. [SR] Shortest Remaining Time First");
                    Console.WriteLine("Unfortunately it has not been implemented yet :/");
                    Console.WriteLine("Good bye!");
                    return false;
                default:
                    Console.WriteLine("Not a valid option.");
                    Console.WriteLine("Good bye!");
                    return false;
            }
        }
        #endregion

        #region Processes
        private bool PromptForMain()
        {
            if (_stagger is null) return this.PromptError("An internal error occurred, please restart the application.");

            do 
            {
                Console.Clear();
                Console.WriteLine("-------------------");
                Console.WriteLine($"You chose {_stagger.Name}.");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Blue;                
                Console.WriteLine($"Processes");
                Console.WriteLine($"Waiting: {_stagger.Arriving.Count}");
                Console.ResetColor();
                
                Console.WriteLine();
                Console.WriteLine("Task #2: Add more processes.");
                Console.WriteLine();
                Console.WriteLine("Choose an option.");
                Console.WriteLine("1. [A]dd new process");
                Console.WriteLine("2. [S]imulate");
                Console.WriteLine("3. [Q]uit");
                Console.Write("option: ");
            } while (this.ProcessThirdOption());

            return true;
        }

        private bool ProcessThirdOption()
        {
            string? option = Console.ReadLine();
            Console.WriteLine();

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
                    Console.WriteLine("Not a valid option.");
                    Console.WriteLine("Good bye!");
                    return false;
            }
        }

        private bool PromptForProcess()
        {
            if (_stagger is null) return this.PromptError("An internal error occurred, please restart the application.");

            Console.Clear();
            Console.WriteLine("-------------------");

            int arrivalTime, priority, steps;
            arrivalTime = this.PromptForProcessProperty("Arrival time", 0, 15);
            priority = this.PromptForProcessProperty("Priority", 0, 5);
            steps = this.PromptForProcessProperty("Steps", 0, 10);

            _stagger.Arrive(new Process(arrivalTime, priority, steps));

            return true;
        }

        private int PromptForProcessProperty(string name, int min, int max)
        {
            int propertyValue;
            Console.Write($"{name}: ");
            while (!int.TryParse(Console.ReadLine(), out propertyValue) || propertyValue < min || propertyValue > max)
            {
                Console.WriteLine($"Invalid {name}! Type an integer number between {min} and {max}.");
                Console.WriteLine();
                Console.Write($"{name}: ");
            }
            Console.WriteLine($"Selected: {propertyValue}");
            Console.WriteLine();
            return propertyValue;
        }
        #endregion

        #region Simulation
        private bool PromptSimulation()
        {
            if (_stagger is null) return this.PromptError("An internal error occurred, please restart the application.");

            int iteration = 0;
            do 
            {
                iteration++;
                this.PromptStep();
                this.PromptSimulationStatus(iteration);
                if (!this.PromptConfirmation()) break;
            } while (_stagger.Busy);

            return PromptStatistics();
        }

        private bool PromptStep()
        {
            if (_stagger is null) return this.PromptError("An internal error occurred, please restart the application.");

            Console.Clear();

            PromptDelay();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            _stagger.Work(Console.WriteLine);
            Console.WriteLine();
            
            Console.ResetColor();
            Console.WriteLine($"Type any key to continue..");
            Console.ReadKey();

            return true;
        }

        private void PromptSimulationStatus(int iteration)
        {
            Console.Clear();
            
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Iteration {iteration}");
            Console.WriteLine();
            
            this.PromptProgress();
            Console.WriteLine();

            this.PromptQueue();
            Console.ResetColor();
        }

        private bool PromptProgress()
        {
            if (_stagger is null) return this.PromptError("An internal error occurred, please restart the application.");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Total steps");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(new string('|', _stagger.Progress));
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(new string('|', _stagger.Length - _stagger.Progress));
            Console.WriteLine();
            Console.ResetColor();

            return true;
        }

        private bool PromptQueue()
        {
            if (!PromptArrivingQueue()) return false;
            if (!PromptWaitingQueue()) return false;
            if (!PromptReadyQueue()) return false;
            if (!PromptCompletedQueue()) return false;

            return true;
        }

        private bool PromptArrivingQueue()
        {
            if (_stagger is null) return this.PromptError("An internal error occurred, please restart the application.");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Arriving Queue");
            foreach (IProcess process in _stagger.Arriving)
            {
                Console.WriteLine($"  PID {process.ID.ToString().PadLeft(4, '0')}");
            }
            Console.WriteLine();
            Console.ResetColor();
            return true;
        }

        private bool PromptWaitingQueue()
        {
            if (_stagger is null) return this.PromptError("An internal error occurred, please restart the application.");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Waiting Queue");
            foreach (IProcess process in _stagger.Waiting)
            {
                Console.Write($"  PID {process.ID.ToString().PadLeft(4, '0')} : ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{new string('|', process.CurrentStep)}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"{new string('|', process.RemainingSteps)}");
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            Console.WriteLine();
            Console.ResetColor();
            return true;
        }

        private bool PromptReadyQueue()
        {
            if (_stagger is null) return this.PromptError("An internal error occurred, please restart the application.");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Ready Queue");
            foreach (IProcess process in _stagger.Ready)
            {
                Console.Write($"  PID {process.ID.ToString().PadLeft(4, '0')} : ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{new string('|', process.CurrentStep)}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"{new string('|', process.RemainingSteps)}");
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            Console.WriteLine();
            Console.ResetColor();
            return true;
        }

        private bool PromptCompletedQueue()
        {
            if (_stagger is null) return this.PromptError("An internal error occurred, please restart the application.");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Completed Queue");
            foreach (IProcess process in _stagger.Completed)
            {
                Console.Write($"  PID {process.ID.ToString().PadLeft(4, '0')} : ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{new string('|', process.CurrentStep)}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"{new string('|', process.RemainingSteps)}");
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            Console.WriteLine();
            Console.ResetColor();
            return true;
        }

        private bool PromptStatistics()
        {
            if (_stagger is null) return this.PromptError("An internal error occurred, please restart the application.");
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Statistics..");
            Console.WriteLine();
            PromptDelay();
            _stagger.ReportStatistics(Console.WriteLine);
            Console.ResetColor();
            Console.Write("Type any key to continue..");
            Console.ReadKey();
            return false;
        }
        #endregion

        #region Utility

        private bool PromptConfirmation()
        {
            Console.WriteLine("Do you wish to proceed?");
            Console.WriteLine("1. [Y]es");
            Console.WriteLine("2. [N]o");
            Console.Write("option: ");

            return this.ProcessConfirmation();
        }

        private bool ProcessConfirmation()
        {
            string option = Console.ReadLine() ?? "N";

            switch (option)
            {
                case "Y":
                case "y":
                case "1":
                    return true;
                case "N":
                case "n":
                case "2":
                    Console.WriteLine("Good bye!");
                    return false;
                default:
                    Console.WriteLine("Not a valid option.");
                    Console.WriteLine("Good bye!");
                    return false;
            }
        }

        private bool PromptError(string message)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(message);
            Console.WriteLine();
            Console.WriteLine("Type any key to continue..");
            Console.ReadKey();

            return false;
        }

        private static void PromptDelay()
        {
            Console.Write($"Working");
            Thread.Sleep(250);
            Console.Write($".");
            Thread.Sleep(250);
            Console.Write($".");
            Thread.Sleep(250);
            Console.WriteLine($".");
            Thread.Sleep(250);
            Console.WriteLine();
        }
        #endregion
    }
}