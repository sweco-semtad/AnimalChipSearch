using System;

namespace SKKSearchAPI
{
    public interface SKKSearch
    {
        /// <summary>
        /// Search dogs
        /// </summary>
        /// <param name="idMode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        AnimalList SearchDogs(IdModell idMode, String id);

        /// <summary>
        /// Search for a specific dog
        /// </summary>
        /// <param name="dog"></param>
        /// <returns></returns>
        Animal GetDogDetails(Animal dog);

        /// <summary>
        /// Search cats
        /// </summary>
        /// <param name="idMode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        AnimalList SearchCats(IdModell idMode, String id);

        /// <summary>
        /// Search for a specific cat
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        Animal GetCatDetails(Animal cat);
    }
}
