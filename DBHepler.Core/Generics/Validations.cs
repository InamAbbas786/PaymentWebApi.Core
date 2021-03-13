using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DBHepler.Core.Generics
{
   public static class Validations
    {
        public static bool ValidateCreditCard(string creditCardNumber)
        {
            //Strip any non-numeric values
            creditCardNumber = Regex.Replace(creditCardNumber, @"[^\d]", "");
            //Build your Regular Expression
            Regex expression = new Regex(@"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$");
            //Return if it was a match or not
            return expression.IsMatch(creditCardNumber);
        }



        public static bool IsValidateExpiryDate(string expiryDate)
        {
            var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            var yearCheck = new Regex(@"^20[0-9]{2}$");

            var dateParts = expiryDate.Split('/'); //expiry date in from MM/yyyy            
            if (!monthCheck.IsMatch(dateParts[0]) || !yearCheck.IsMatch(dateParts[1])) // <3 - 6>
                return false; // ^ check date format is valid as "MM/yyyy"

            var year = int.Parse(dateParts[1]);
            var month = int.Parse(dateParts[0]);
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month); //get actual expiry date
            var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

            //check expiry greater than today & within next 5 years because of the international standard any bank not allowed credit card more then 5 year
            return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(5));
        }
        public static bool IsValidateCvv(string cvv)
        {
            bool valid = true;
            var cvvCheck = new Regex(@"^\d{3}$");
            if (!cvvCheck.IsMatch(cvv)) // <2>check cvv is valid as "999"
                valid= false;
            return valid;
        }
        public static bool IsValidatesCreditCard(string cardNo)
        {
            bool valid = true;
            var cardCheck = new Regex(@"^(1298|1267|4512|4567|8901|8933)([\-\s]?[0-9]{4}){3}$");
            if (!cardCheck.IsMatch(cardNo)) // <1>check card number is valid
                valid= false;
            return valid;
        }

        public static bool IsCreditCardInfoValid(string cardNo, string expiryDate, string cvv)
        {
            var cardCheck = new Regex(@"^(1298|1267|4512|4567|8901|8933)([\-\s]?[0-9]{4}){3}$");
            var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            var yearCheck = new Regex(@"^20[0-9]{2}$");
            var cvvCheck = new Regex(@"^\d{3}$");

            if (!cardCheck.IsMatch(cardNo)) // <1>check card number is valid
                return false;
            if (!cvvCheck.IsMatch(cvv)) // <2>check cvv is valid as "999"
                return false;

            var dateParts = expiryDate.Split('/'); //expiry date in from MM/yyyy            
            if (!monthCheck.IsMatch(dateParts[0]) || !yearCheck.IsMatch(dateParts[1])) // <3 - 6>
                return false; // ^ check date format is valid as "MM/yyyy"

            var year = int.Parse(dateParts[1]);
            var month = int.Parse(dateParts[0]);
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month); //get actual expiry date
            var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

            //check expiry greater than today & within next 5 years because of the international standard any bank not allowed credit card more then 5 year
            return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(5));
        }
    }
}
