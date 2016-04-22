using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NCodeRiddian
{
    public interface Pathable
    {
        /// <summary>
        /// Returns the value (Lower = favored) of moving from the previous pathable to the current pathable
        /// </summary>
        /// <param name="previous">The previous step in the path</param>
        /// <returns></returns>
        float getMoveScore(Pathable previous);

        /// <summary>
        /// Returns the vale (Lower = favored) of the heuristic score from the current pathable to the target pathable
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        float getHeuristicScore(Pathable target);

        /// <summary>
        /// Returns the centered rectangle of this object
        /// </summary>
        /// <returns></returns>
        Rectangle getTrueRectangle();

        /// <summary>
        /// Return whether or not this pathable is still valid
        /// </summary>
        /// <returns></returns>
        bool isValid();

        /// <summary>
        /// Returns a list of all Pathables that could be reached from this Pathable
        /// </summary>
        /// <returns></returns>
        List<Pathable> getConnected();
    }
}