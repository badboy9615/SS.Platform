//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SS.Platform.OA.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee
    {
        public Employee()
        {
            this.Family = new HashSet<Family>();
            this.Train = new HashSet<Train>();
            this.Experience = new HashSet<Experience>();
        }
    
        public int ID { get; set; }
        public int SubBy { get; set; }
        public System.DateTime SubTime { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedTIme { get; set; }
        public short DelFlag { get; set; }
        public bool TakeEffect { get; set; }
        public System.DateTime TakeEffectTime { get; set; }
        public System.DateTime LoseEffectTime { get; set; }
        public string Remark { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int xyd { get; set; }
        public Nullable<int> dj { get; set; }
        public int xb { get; set; }
        public Nullable<int> wh { get; set; }
        public Nullable<int> zzmm { get; set; }
        public string zp { get; set; }
        public int nl { get; set; }
        public Nullable<int> sx { get; set; }
        public Nullable<System.DateTime> csrq { get; set; }
        public Nullable<int> hyzk { get; set; }
        public Nullable<int> sg { get; set; }
        public Nullable<int> qwgz { get; set; }
        public Nullable<int> grxy { get; set; }
        public Nullable<int> jkzk { get; set; }
        public Nullable<int> tz { get; set; }
        public Nullable<bool> sfzj { get; set; }
        public Nullable<int> xm { get; set; }
        public Nullable<int> yx { get; set; }
        public Nullable<int> zzqy { get; set; }
        public Nullable<int> zxsj { get; set; }
        public Nullable<int> gzfw { get; set; }
        public Nullable<int> gznx { get; set; }
        public string sfzh { get; set; }
        public string sfzt { get; set; }
        public string xxly { get; set; }
        public string sjhm { get; set; }
        public string jtzz { get; set; }
        public string wxh { get; set; }
        public string xzzz { get; set; }
        public string bz { get; set; }
        public bool jkzs { get; set; }
        public Nullable<System.DateTime> yxqz { get; set; }
        public bool myhlz { get; set; }
        public bool yyszgz { get; set; }
        public System.DateTime lrsj { get; set; }
        public Nullable<int> gzzt { get; set; }
        public string jbkh { get; set; }
        public string qwjob { get; set; }
    
        public virtual ICollection<Family> Family { get; set; }
        public virtual ICollection<Train> Train { get; set; }
        public virtual ICollection<Experience> Experience { get; set; }
    }
}
