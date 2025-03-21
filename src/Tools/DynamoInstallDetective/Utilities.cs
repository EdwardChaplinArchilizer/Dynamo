using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace DynamoInstallDetective
{
    /// <summary>
    /// Utility class for install detective
    /// </summary>
#if NET6_0_OR_GREATER
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
    public static class Utilities
    {
        /// <summary>
        /// Finds all unique Dynamo installations on the system
        /// </summary>
        /// <param name="additionalDynamoPath">Additional path for Dynamo binaries
        /// to be included in search</param>
        /// <returns>List of KeyValuePair of install location and version info 
        /// as Tuple. The returned list is sorted based on version info.</returns>
        public static IEnumerable FindDynamoInstallations(string additionalDynamoPath)
        {
            var installs = DynamoProducts.FindDynamoInstallations(additionalDynamoPath);
            return
                installs.Products.Select(
                    p =>
                        new KeyValuePair<string, Tuple<int, int, int, int>>(
                        p.InstallLocation,
                        p.VersionInfo));
        }

        /// <summary>
        /// Finds all unique Dynamo installation on the system that has file 
        /// identifiable by the given fileLocator.
        /// </summary>
        /// <param name="additionalDynamoPath">Additional path for Dynamo binaries
        /// to be included in search</param>
        /// <param name="fileLocator">A callback method to locate dynamo specific files.</param>
        /// <returns>List of KeyValuePair of install location and version info 
        /// as Tuple. The returned list is sorted based on version info.</returns>
        public static IEnumerable LocateDynamoInstallations(string additionalDynamoPath, Func<string, string> fileLocator)
        {
            var installs = DynamoProducts.FindDynamoInstallations(additionalDynamoPath, new InstalledProductLookUp("Dynamo", fileLocator));
            return
                installs.Products.Select(
                    p =>
                        new KeyValuePair<string, Tuple<int, int, int, int>>(
                        p.InstallLocation,
                        p.VersionInfo));
        }

        /// <summary>
        /// Finds all products installed on the system with given product name
        /// search pattern and file name search pattern. e.g. to find Dynamo
        /// installations, we can use Dynamo as product search pattern and
        /// DynamoCore.dll as file search pattern.
        /// </summary>
        /// <param name="productSearchPattern">search key for product</param>
        /// <param name="fileSearchPattern">search key for files</param>
        /// <returns>List of KeyValuePair of product install location and 
        /// version info as Tuple of the file found in the installation based 
        /// on file search pattern. The returned list is sorted based on version 
        /// info.</returns>
        public static IEnumerable FindProductInstallations(string productSearchPattern, string fileSearchPattern)
        {
            var installs = new InstalledProducts();
            installs.LookUpAndInitProducts(new InstalledProductLookUp(productSearchPattern, fileSearchPattern));

            return
                installs.Products.Select(
                    p =>
                        new KeyValuePair<string, Tuple<int, int, int, int>>(
                        p.InstallLocation,
                        p.VersionInfo));
        }

        /// <summary>
        /// Finds all products installed on the system with given product name
        /// search pattern and file name search pattern. e.g. to find Dynamo
        /// installations, we can use Dynamo as product search pattern and
        /// DynamoCore.dll as file search pattern.
        /// </summary>
        /// <param name="productSearchPattern">search keys for product</param>
        /// <param name="fileSearchPattern">search key for files</param>
        /// <returns>List of KeyValuePair of product install location and 
        /// version info as Tuple of the file found in the installation based 
        /// on file search pattern. The returned list is sorted based on version 
        /// info.</returns>
        public static IEnumerable FindMultipleProductInstallations(List<string> productSearchPatterns, string fileSearchPattern)
        {
            using (RegUtils.StartCache())
            {
                var installs = new InstalledProducts();

                // Look for ASM in installed ASC packages
                installs.LookUpAndInitProducts(new InstalledAscLookUp(fileSearchPattern));

                // Look up products with ASM installed on user's computer
                foreach (var productSearchPattern in productSearchPatterns)
                {
                    installs.LookUpAndInitProducts(new InstalledProductLookUp(productSearchPattern, fileSearchPattern));
                }

                return
                    installs.Products.Select(
                        p =>
                            new KeyValuePair<string, Tuple<int, int, int, int>>(
                            p.InstallLocation,
                            p.VersionInfo));
            }
        }
    }
}
