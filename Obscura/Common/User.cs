using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscura.Common {

    public class User {
        private int _id;
        private DateTimeSet _dates;
        private string _username, _lastname, _firstname, _email;

        #region accessors

        public int Id {
            get { return _id; }
        }

        public string Username {
            get { return _username; }
            set {
                Update(value, null, null, null);
                _username = value;
            }
        }

        public string Lastname {
            get { return _lastname; }
            set {
                Update(null, value, null, null);
                _lastname = value;
            }
        }

        public string Firstname {
            get { return _firstname; }
            set {
                Update(null, null, value, null);
                _firstname = value;
            }
        }

        public string Email {
            get { return _email; }
            set {
                Update(null, null, null, value);
                _email = value;
            }
        }

        #endregion

        public User(int id) {
            //TODO: constructor
        }

        public void Update(string username, string lastname, string firstname, string email) {
            Update(Id, username, lastname, firstname, email);
        }

        private void Update(int id, string username, string lastname, string firstname, string email) {
            //TODO: update
            //TODO: update dates
        }

        public static User Create() {
            //TODO: Create()
            return null;
        }

        public static User Authenticate(string username, string password) {
            //TODO: Authenticate()
            return null;
        }
    }
}
