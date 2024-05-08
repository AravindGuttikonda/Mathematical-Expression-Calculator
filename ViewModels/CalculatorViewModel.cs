using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_Expression_Calculator.Models;

namespace WPF_Expression_Calculator.ViewModels
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private CalculatorModel _model;

        public CalculatorModel Model
        {
            get { return _model; }
            set 
            { 
                _model = value; 
                OnPropertyChanged(nameof(Model));
            }
        }

        public ICommand EvaluateCommand { get; set; }
        public ICommand ClearCommand { get; set; }

        public CalculatorViewModel()
        {
            Model = new CalculatorModel();
            EvaluateCommand = new RelayCommand(EvaluateExpression);
            ClearCommand = new RelayCommand(Clear);
        }

        private int Preference(string key)
        {
            if (key == "+" || key == "-")
            {
                return 1;
            }
            if (key == "/" || key == "*")
            {
                return 2;
            }
            return 0;
        }
        private void EvaluateExpression()
        {
            string value = Model.TextValue;
            char[] chars = value.ToCharArray();
            List<string> list = new List<string>();
            int d = 0;
            while (d < chars.Length - 1)
            {
                if (chars[d] == '+' || chars[d] == '-' || chars[d] == '*' || chars[d] == '/' || chars[d] == '(' || chars[d] == ')')
                {
                    list.Add(chars[d].ToString());
                    d++;
                }
                else
                {
                    string x = chars[d].ToString();
                    while (d < chars.Length - 1 && (chars[d + 1] == '1' || chars[d + 1] == '2' || chars[d + 1] == '3' || chars[d + 1] == '4' || chars[d + 1] == '5' || chars[d + 1] == '6' || chars[d + 1] == '7' || chars[d + 1] == '8' || chars[d + 1] == '9' || chars[d + 1] == '0' || chars[d + 1] == '.'))
                    {
                        x += chars[d + 1].ToString();
                        d++;
                    }
                    list.Add(x);
                    d++;
                }
            }
            if (chars.Length == 1)
            {
                list.Add(chars[d].ToString());
            }
            if (d < chars.Length && chars[d] == ')')
            {
                list.Add(chars[d].ToString());
            }
            else if (d < chars.Length && chars.Length>1)
            {
                if (chars[d - 1] == '+' || chars[d - 1] == '-' || chars[d - 1] == '*' || chars[d - 1] == '/')
                {
                    list.Add(chars[d].ToString());
                }
                else
                {
                    list[list.Count - 1] = list[list.Count - 1] + chars[d].ToString();
                }
            }
            
            List<string> postfix = new List<string>();
            Stack<string> ps = new Stack<string>();
            foreach (string str in list)
            {
                if (str == "+" || str == "-" || str == "*" || str == "/")
                {
                    while ((ps.Count > 0) && (Preference(ps.Peek()) >= Preference(str)))
                    {
                        string f = ps.Pop();
                        postfix.Add(f);
                    }
                    ps.Push(str);
                }
                else if (str == "(")
                {
                    ps.Push(str);
                }
                else if (str == ")")
                {
                    while (ps.Peek() != "(")
                    {
                        string g = ps.Pop();
                        postfix.Add(g);
                    }
                    ps.Pop();
                }
                else
                {
                    postfix.Add(str);
                }
            }
            if (ps.Count > 0)
            {
                while (ps.Count > 0)
                {
                    string f = ps.Pop();
                    if (f != "(")
                    {
                        postfix.Add(f);
                    }
                }
            }
            Stack<double> st = new Stack<double>();
            foreach (string str in postfix)
            {
                if (str == "+" || str == "-" || str == "*" || str == "/")
                {
                    double b = st.Pop();
                    double a = st.Pop();
                    if (str == "+")
                    {
                        st.Push(a + b);
                    }
                    else if (str == "-")
                    {
                        st.Push(a - b);
                    }
                    else if (str == "*")
                    {
                        st.Push(a * b);
                    }
                    else if (str == "/")
                    {
                        st.Push(a / b);
                    }
                }
                else
                {
                    double x = double.Parse(str);
                    st.Push(x);
                }
            }

            Model.TextValue = st.Peek().ToString();
            OnPropertyChanged(nameof(Model));
        }

        private void Clear()
        {
            Model.TextValue = string.Empty; 
            OnPropertyChanged(nameof(Model));
        }
    }
}
