namespace NCodeRiddian
{
    /// <summary>
    /// A path that is currently being traversed
    /// </summary>
    public class ActivePath : Path
    {
        /// <summary>
        /// Create an active path from a path
        /// </summary>
        /// <param name="p">The path</param>
        /// <returns>The active path</returns>
        public static ActivePath MakeActive(Path p)
        {
            ActivePath np = new ActivePath();
            np.path = p.GetPath();
            np.currentlyReached = null;
            np.isPathDone = false;
            return np;
        }

        private Pathable currentlyReached;
        private bool isPathDone;

        /// <summary>
        /// Gets the pathable currently next to move to
        /// </summary>
        /// <returns>The next pathable</returns>
        public Pathable getNext()
        {
            if (isPathDone)
                return base.getLast();
            if (currentlyReached == null)
                return getFirst();
            else
                return getNext(currentlyReached);
        }

        /// <summary>
        /// The next pathable was reached - advance the path
        /// </summary>
        public void reachedNext()
        {
            if (isPathDone)
                return;
            if (currentlyReached == null)
                currentlyReached = getFirst();
            if (currentlyReached == base.getLast())
                isPathDone = true;
            else
                currentlyReached = base.getNext(currentlyReached);
        }

        /// <summary>
        /// The specified pathable was reached - advance the path
        /// </summary>
        /// <param name="p">Which pathable was reached</param>
        public void reached(Pathable p)
        {
            if (isPathDone)
                return;
            if (p == base.getLast())
                isPathDone = true;
            else
                currentlyReached = base.getNext(p);
        }

        /// <summary>
        /// Is this path finished?
        /// </summary>
        /// <returns></returns>
        public bool amDone()
        {
            return isPathDone;
        }
    }
}