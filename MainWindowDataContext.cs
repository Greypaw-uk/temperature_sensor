using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;

namespace Temperature_Monitor
{
    class MainWindowDataContext
    {
        public static ObservableCollection<Person> PersonList { get; set; } = new ObservableCollection<Person>();

        public static ObservableCollection<Person> PersonListWarning { get; set; } = new ObservableCollection<Person>();
    }

    public class Person
    {
        private string name;
        private int age;
        private decimal temp;
        public string Name
        {
            get { return name;}
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("name"));
            }
        }

        public int Age
        {
            get { return age; }
            set
            {
                age = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("age"));
            }
        }

        public decimal Temperature
        {
            get { return temp; }
            set
            {
                temp = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("temp"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
