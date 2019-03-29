﻿using AntDeploy.Models;
using AntDeployWinform.Models;
using AntDeployWinform.Winform;

using System;
using System.IO;
using System.Linq;

namespace AntDeploy.Commands
{
    internal sealed class EditProjectCommand : BaseCommand
    {
        public static EditProjectCommand Instance { get; private set; }

        public static void Initialize(EditProjectPackage package)
        {
            Instance = new EditProjectCommand(package);
            package.CommandService.AddCommand(Instance);
        }

        private string _projectFile;
        private EnvDTE.Project _project;

        private EditProjectCommand(EditProjectPackage package)
            : base(package, Ids.CMD_SET, Ids.EDIT_PROJECT_MENU_COMMAND_ID)
        {
        }

        protected override void OnBeforeQueryStatus()
        {


            var projects = SelectedProjects.ToArray();
            if (projects.Length == 1)
            {
                _project = projects[0];

                _projectFile = _project.FullName;
                Text = "AntDeploy";
                Visible = true;
                return;

                //var project = projects[0];
                //if (ProjectHelper.IsDotNetCoreProject(project))
                //{
                //    _projectFile = project.FullName;
                //    Text = "AntDeploy";
                //    Visible = true;

                //}
                //else
                //{
                //    Visible = false;
                //}
            }
            else
            {
                Visible = false;
            }
        }

        protected override void OnExecute()
        {
            //RootNamespace Title Product OutputFileName
            //var friendlyName = "antDomain";
            //var assembly = Assembly.GetExecutingAssembly();
            //var codeBase = assembly.Location;
            //var codeBaseDirectory = Path.GetDirectoryName(codeBase);
            //var setup = new AppDomainSetup()
            //{
            //    ApplicationName = "AntDeployApplication",
            //    ApplicationBase = codeBaseDirectory,
            //    DynamicBase = codeBaseDirectory,
            //};
            //setup.CachePath = setup.ApplicationBase;
            //setup.ShadowCopyFiles = "true";
            //setup.ShadowCopyDirectories = setup.ApplicationBase;
            //AppDomain.CurrentDomain.SetShadowCopyFiles();
            //SecurityZone zone = SecurityZone.MyComputer;
            //Evidence baseEvidence = AppDomain.CurrentDomain.Evidence;
            //Evidence evidence = new Evidence(baseEvidence);
            //string assemblyName = Assembly.GetExecutingAssembly().FullName;
            //evidence.AddAssembly(assemblyName);
            //evidence.AddHost(new Zone(zone));

            //AppDomain otherDomain = AppDomain.CreateDomain(friendlyName,evidence, setup);
            try
            {

                ProjectParam param = new ProjectParam();
                param.IsWebProejct = ProjectHelper.IsWebProject(_project);
                param.IsNetcorePorject = ProjectHelper.IsDotNetCoreProject(_project);
                param.OutPutName = _project.GetProjectProperty("OutputFileName");
                param.VsVersion = ProjectHelper.GetVsVersion();
                param.MsBuildPath = ProjectHelper.GetMsBuildPath();
                if (!string.IsNullOrEmpty(param.MsBuildPath))
                {
                    param.MsBuildPath = Path.Combine(param.MsBuildPath, "MSBuild.exe");
                }
                Deploy deploy = new Deploy(_projectFile, param);
                deploy.ShowDialog();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //AppDomain.Unload(otherDomain);
            }
        }

    }



}