// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConvertIdToPath.cs" company="Sitecore A/S">
//  Copyright (C) by Sitecore A/S
// </copyright>
// <summary>
//   The convert id to path.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using JetBrains.ReSharper.Psi.Resx.Utils;
using Sitecore.VisualStudio.Extensions.DataServiceExtensions;
using Sitecore.VisualStudio.Extensions.XElementExtensions;
using Sitecore.VisualStudio.Shell.Environment;

namespace Sitecore.Rocks.Resharper.TextEditors.Actions
{
    using System;
    using JetBrains.Application.Progress;
    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.Feature.Services.Bulbs;
    using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
    using JetBrains.ReSharper.Feature.Services.LinqTools;
    using JetBrains.ReSharper.Intentions.Extensibility;
    using JetBrains.ReSharper.Psi.CSharp;
    using JetBrains.ReSharper.Psi.Tree;
    using JetBrains.TextControl;
    using JetBrains.Util;
    using Sitecore.Rocks.Extensions.DataProviderExtensions;
    using Sitecore.VisualStudio.Annotations;
    using Sitecore.VisualStudio.Data;
    using Sitecore.VisualStudio.Data.DataServices;
    using Sitecore.VisualStudio.Diagnostics;
    using Sitecore.VisualStudio.Extensions.StringExtensions;
    using Sitecore.VisualStudio.Sites;

    /// <summary>The convert id to path.</summary>
    [ContextAction(Name = "Convert Sitecore Path to ItemID", Description = "Convert's string Path to a Sitecore ID.", Group = "C#")]
    public class ConvertPathToId : ContextActionBase
    {
        #region Fields

        /// <summary>The _provider.</summary>
        private readonly ICSharpContextActionDataProvider provider;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="ConvertIdToPath"/> class.</summary>
        /// <param name="provider">The provider.</param>
        public ConvertPathToId([NotNull] ICSharpContextActionDataProvider provider)
        {
            Assert.ArgumentNotNull(provider, "provider");

            this.provider = provider;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the site.
        /// </summary>
        /// <value>The site.</value>
        [NotNull]
        public Site Site { get; private set; }



        /// <summary>
        /// Gets the string literal.
        /// </summary>
        /// <value>The string literal.</value>
        [NotNull]
        public ILiteralExpression StringLiteral { get; private set; }

        /// <summary>Gets the text.</summary>
        public override string Text
        {
            get
            {
                return "Convert Sitecore Path to Item ID";
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>The is available.</summary>
        /// <param name="cache">The cache.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool IsAvailable(IUserDataHolder cache)
        {
            Assert.ArgumentNotNull(cache, "cache");
            var literal = this.provider.GetSelectedElement<ILiteralExpression>(true, true);
            if (literal == null || !literal.IsConstantValue() || !literal.ConstantValue.IsString())
            {
                return false;
            }

            var text = literal.ConstantValue.Value as string;
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            if (!text.StartsWith("/sitecore"))
            {
                return false;
            }

            var project = this.provider.GetProject();
            if (project == null)
            {
                return false;
            }

            var site = project.Site;
            if (site == null)
            {
                return false;
            }

            this.Site = site;
            this.StringLiteral = literal;
            this.Path = text;

            return true;
        }

        #endregion

        #region Methods

        /// <summary>The execute psi transaction.</summary>
        /// <param name="solution">The solution.</param>
        /// <param name="progress">The progress.</param>
        /// <returns>The <see cref="Action"/>.</returns>
        protected override Action<ITextControl> ExecutePsiTransaction([NotNull] ISolution solution, [NotNull] IProgressIndicator progress)
        {
            Debug.ArgumentNotNull(solution, "solution");
            Debug.ArgumentNotNull(progress, "progress");
            var busy = true;

            var returnValue = string.Empty;
            var dbQuery = string.Format("Select @@ID from {0}", Path.TrimEnd('/'));
            var thisItem = new ItemUri(new DatabaseUri(Site, new DatabaseName("master")), new ItemId(new Guid("{11111111-1111-1111-1111-111111111111}")));
            new ServerHost().QueryAnalyzer.Run(thisItem, dbQuery, "1",
                delegate(string response, ExecuteResult result)
                {
                    if (!DataService.HandleExecute(response, result, true))
                    {
                        busy = false;
                        return;
                    }

                    var root = response.ToXElement();
                    if (root == null)
                    {
                        busy = false;
                        return;
                    }
                    if (!root.XPathSelectElement("//value").IsEmpty)
                    {
                        returnValue = root.XPathSelectElement("//value").Value;                        
                    }

                    busy = false;
                });
         
            while (busy)
            {
                AppHost.DoEvents();
            }

            var factory = CSharpElementFactory.GetInstance(provider.PsiModule);
            var expression = factory.CreateExpressionAsIs("\"" + returnValue + "\"");
            this.StringLiteral.ReplaceBy(expression);

            return null;
        }

        [CanBeNull]
        private static XElement Parse([NotNull] string xml)
        {
            XDocument document;
            Debug.ArgumentNotNull(xml, "xml");
            try
            {
                document = XDocument.Parse(xml);
            }
            catch
            {
                return null;
            }
            return document.Root;
        }


        #endregion
    }
}