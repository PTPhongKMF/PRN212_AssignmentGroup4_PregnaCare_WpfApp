using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PregnaCare_WpfApp.Utils {
    public static class UserSession {
        public static Guid Id { get; set; }

        // call .clear() when logging out
        public static void Clear() {
            Id = Guid.Empty;
        }
    }
}
