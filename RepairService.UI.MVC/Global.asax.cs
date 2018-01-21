using Microsoft.AspNet.Identity;
using RepairService.BLL.Account;
using RepairService.Entity.Enums;
using RepairService.Entity.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RepairService.UI.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var roleManager = MembershipTools.NewRoleManager();
            var roller = Enum.GetNames(typeof(IdentityRoles));
            foreach (var rol in roller)
            {
                if (!roleManager.RoleExists(rol))
                    roleManager.Create(new ApplicationRole()
                    {
                        Name = rol
                    });
            }

        }
    }
}
