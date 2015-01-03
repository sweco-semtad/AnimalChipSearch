using System;

namespace SKKSearchAPI
{
    public interface SKKSearch
    {
        /// <summary>
        /// Search dogs
        /// </summary>
        /// <param name="inkId"></param>
        /// <param name="chiId"></param>
        /// <returns></returns>
        AnimalList SearchDogs(String inkId, String chiId);

        /// <summary>
        /// Search for a specific dog
        /// </summary>
        /// <param name="dog"></param>
        /// <returns></returns>
        Animal GetDogDetails(Animal dog);

        /// <summary>
        /// Search cats
        /// </summary>
        /// <param name="inkId"></param>
        /// <param name="chiId"></param>
        /// <returns></returns>
        AnimalList SearchCats(String inkId, String chiId);

        /// <summary>
        /// Search for a specific cat
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        Animal GetCatDetails(Animal cat);
    }
}
