using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 谁是卧底.util
{
    internal class MainBase
    {
        public string Message { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string[] Conn { get; set; }
        public string Info { get; set; }

        public string[] OrdinaryName { get; set; }
        public string Ordinarykw { get; set; }
        public string UndercoverName { get; set; }
        public string Undercoverkw { get; set; }
        public string KwType { get; set; }

        public string VoteName { get; set; }
        public string PassedName { get; set; }
//        {
//  Type: 'serverMessage',
//  Info: 'sendKw',
//  KwType: '生活',
//  ordinaryName: [],
//  ordinaryKw: '板砖',
//  undercoverName: '123',
//  undercoverKw: '瓷砖'
//}
}
}
