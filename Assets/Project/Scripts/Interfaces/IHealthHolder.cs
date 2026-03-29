using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IHealthHolder
{
    int HealthPoints { get; set; }
    void OnDamage(); // temporary!!! it will be implement IDamageable interface. Current interface must be used for UI health bar
}
