using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITryExpenseTracker.Scanner; 
public class ScannerExceptions : Exception {

    public ScannerExceptions(string message) : base(message) { }

    public ScannerExceptions(string message, Exception e) : base(message, e) { }

}
