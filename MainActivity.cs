using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
namespace Calculator {
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : AppCompatActivity {
        private TextView displayTextView;
        private string currentInput = "0";
        private string previousInput = "";
        private string currentOperation = "";
        private bool lastWasOperation = false;
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            displayTextView = FindViewById<TextView>(Resource.Id.display);
            int[] numberButtonIds = { Resource.Id.btn0, Resource.Id.btn1, Resource.Id.btn2, Resource.Id.btn3, Resource.Id.btn4, Resource.Id.btn5, Resource.Id.btn6, Resource.Id.btn7, Resource.Id.btn8, Resource.Id.btn9 };
            foreach (int id in numberButtonIds) {
                FindViewById<Button>(id).Click += (s, e) => OnNumberClick(((Button)s).Text);
            }
            FindViewById<Button>(Resource.Id.btnAdd).Click += (s, e) => OnOperationClick("+");
            FindViewById<Button>(Resource.Id.btnSubtract).Click += (s, e) => OnOperationClick("-");
            FindViewById<Button>(Resource.Id.btnMultiply).Click += (s, e) => OnOperationClick("×");
            FindViewById<Button>(Resource.Id.btnDivide).Click += (s, e) => OnOperationClick("÷");
            FindViewById<Button>(Resource.Id.btnDecimal).Click += (s, e) => OnDecimalClick();
            FindViewById<Button>(Resource.Id.btnClear).Click += (s, e) => OnClearClick();
            FindViewById<Button>(Resource.Id.btnDelete).Click += (s, e) => OnDeleteClick();
            FindViewById<Button>(Resource.Id.btnEquals).Click += (s, e) => OnEqualsClick();
            UpdateDisplay();
        }
        private void OnNumberClick(string number) {
            if (currentInput == "0" || lastWasOperation) {
                currentInput = number;
                lastWasOperation = false;
            } else {
                currentInput += number;
            }
            UpdateDisplay();
        }
        private void OnOperationClick(string operation) {
            if (!lastWasOperation && currentInput != "") {
                if (previousInput != "" && !lastWasOperation) {
                    CalculateResult();
                }
                previousInput = currentInput;
            }
            currentOperation = operation;
            lastWasOperation = true;
        }
        private void OnDecimalClick() {
            if (!currentInput.Contains(".")) {
                if (currentInput == "" || lastWasOperation) {
                    currentInput = "0.";
                    lastWasOperation = false;
                } else {
                    currentInput += ".";
                }
            }
            UpdateDisplay();
        }
        private void OnClearClick() {
            currentInput = "0";
            previousInput = "";
            currentOperation = "";
            lastWasOperation = false;
            UpdateDisplay();
        }
        private void OnDeleteClick() {
            if (currentInput.Length > 1) {
                currentInput = currentInput.Substring(0, currentInput.Length - 1);
            } else {
                currentInput = "0";
            }
            UpdateDisplay();
        }
        private void OnEqualsClick() {
            if (previousInput != "" && currentOperation != "" && currentInput != "") {
                CalculateResult();
                currentOperation = "";
                lastWasOperation = true;
            }
        }
        private void CalculateResult() {
            try {
                double prev = double.Parse(previousInput);
                double current = double.Parse(currentInput);
                double result = 0;
                result = currentOperation switch {
                    "+" => prev + current,
                    "-" => prev - current,
                    "×" => prev * current,
                    "÷" => current != 0 ? prev / current : throw new DivideByZeroException(),
                    _ => current
                };
                currentInput = result.ToString();
                previousInput = "";
            } catch (DivideByZeroException) {
                currentInput = "Error";
            } catch {
                currentInput = "Error";
            }
        }
        private void UpdateDisplay() {
            displayTextView.Text = currentInput;
        }
    }
}