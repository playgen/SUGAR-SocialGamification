using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGen.SGA.ServerAuthentication.Providers
{
    public class PasswordValidation
    {
        private readonly string _validatePassword;
        private readonly string _validateAgainstPassword;
        private bool _isValid = false;

        public bool IsValid => _isValid;

        public PasswordValidation(string validatePassword, string validateAgainstPassword)
        {
            _validatePassword = validatePassword;
            _validateAgainstPassword = validateAgainstPassword;

            Validate();
        }

        public void Validate()
        {
            _isValid = _validatePassword == _validateAgainstPassword;
        }
    }
}
