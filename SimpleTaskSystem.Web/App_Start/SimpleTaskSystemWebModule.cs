using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Web.Mvc;

namespace SimpleTaskSystem.Web
{
    [DependsOn(
        typeof(AbpWebMvcModule),
        typeof(SimpleTaskSystemDataModule), 
        typeof(SimpleTaskSystemApplicationModule), 
        typeof(SimpleTaskSystemWebApiModule))]
    public class SimpleTaskSystemWebModule : AbpModule
    {
        //配置ABP是在模块的PreInitialize事件中完成的。
        public override void PreInitialize()
        {
            //Add/remove languages for your application
            //为应用添加语言
            Configuration.Localization.Languages.Add(new LanguageInfo("en", "English", "famfamfam-flag-england", true));
            Configuration.Localization.Languages.Add(new LanguageInfo("tr", "Türkçe", "famfamfam-flag-tr"));
            Configuration.Localization.Languages.Add(new LanguageInfo("zh-CN", "简体中文", "famfamfam-flag-cn"));

            //Add/remove localization sources here
            //添加本地化资源
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    SimpleTaskSystemConsts.LocalizationSourceName,
                    new XmlFileLocalizationDictionaryProvider(
                        HttpContext.Current.Server.MapPath("~/Localization/SimpleTaskSystem")
                        )
                    )
                );

            //Configure navigation/menu
            //配置导航菜单
            Configuration.Navigation.Providers.Add<SimpleTaskSystemNavigationProvider>();
        }

        /* NOTE: 
         *  模块介绍
         *  ABP提供了构建模块并将这些模块组合起来创建应用的基础设施。一个模块可以依赖另一个模块。
         *  一般来说，一个程序集可以认为是一个模块。一个模块是由一个派生了AbpModule的类定义的。
         *  比如说我们在开发一个可以用在不同的应用中的博客模块。最简单的模块定义如下：
         *  ABP扫描所有的程序集，并找出所有的派生自AbpModule基类的类。
         *  如果你创建了不止一个程序集的应用，那么建议为每个程序集创建一个模块定义。
         */
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
