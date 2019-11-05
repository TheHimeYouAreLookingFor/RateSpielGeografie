using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateSpielGeografie
{
    class Question
    {
        Random r = new Random();
        public String AN;
        public int CORR;
        public String[] QS, QQ;
        public String HFLAG;
        public Question(String a, String b, String c, String d)
        {
            QS = new String[] {a, b, c, d };
            CORR = r.Next(0, 3);
            AN = QS[CORR];
        }

        public Question(String a, String b, String c, String d, String aa, String bb, String cc, String dd)
        {
            QS = new String[] { a, b, c, d};
            QQ = new String[] { aa, bb, cc, dd};
            CORR = r.Next(0, 3);
            AN = QS[CORR];
            HFLAG = QQ[CORR];
        }
    }
}
