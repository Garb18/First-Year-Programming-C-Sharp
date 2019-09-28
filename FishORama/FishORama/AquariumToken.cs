using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;              // Required to use XNA features.
using XNAMachinationisRatio;                // Required to use the XNA Machinationis Ratio Engine.
using XNAMachinationisRatio.Resource;       // Required to use the XNA Machinationis Ratio resource management features.

namespace FishORama
{
    /// <summary>
    /// Abstraction to represent the Aquarium used in the simulation. The aquarium
    /// is a container for other objects representing creatures that inhabit it and
    /// more. Hence, the aquarium object serves to store references to these objects,
    /// and can be used to mediate their interactions (for instance, an object A
    /// in the aquarium could "ask" to the aquarium to obtain access to another
    /// object B, in order to interact with it).
    /// 
    /// This class is derived from class X2DToken. In the XNA Machinationis Ratio engine
    /// class X2DToken is a base class for all classes representing objects which
    /// have a visual representation and interactive behaviors in a 2D simulation.
    /// X2DToken implements a number of functionalities that make it easy for developers
    /// to add interactivity to objects minimizing the amount of coded required.
    /// 
    /// Hence, whenever we want to create a new type of object, we must create a new
    /// class derived from X2DToken.
    /// </summary>
    /// 
    class AquariumToken : X2DToken
    {
        #region Data Members
        
        // Reference to the simulation kernel (orchestrator of the whole application).
        private Kernel mKernel;             
        
        // Reference to the mind of the aquarium.
        private AquariumMind mMind;         
        
        /*
         * Attributes of the aquarium.
         */
        private int mWidth;                 // Aquarium width.
        private int mHeight;                // Aquarium height.
        
        /*
         * Useful references to entities populating the aquarium.
         */

        // Reference to the chicken leg. Required to allow the piranha
        // reaching it.
        private ChickenLegToken mChickenLeg = null;

        #endregion

        #region Properties

        /// <summary>
        /// Get reference to aquarium width.
        /// </summary>
        public int Width
        {
            get { return mWidth; }
        }

        /// <summary>
        /// Get reference to aquarium height.
        /// </summary>
        public int Height
        {
            get { return mHeight; }
        }

        /// <summary>
        /// Get/set reference to chicken leg.
        /// </summary>
        public ChickenLegToken ChickenLeg
        {
            get { return mChickenLeg; }
            set { mChickenLeg = value; }
        }

        /// <summary>
        /// Get reference to Kernel.
        /// </summary>
        public Kernel Kernel
        {
            get { return mKernel; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for the aquarium.
        /// Uses base class to initialize the token name, and adds code to
        /// initialize custom members.
        /// </summary
        /// <param name="pTokenName">Name of the token.</param>
        /// <param name="pKernel">Reference to the simulation kernel.</param>
        /// <param name="pWidth">Width of the aquarium.</param>
        /// <param name="pHeight">Height of the aquarium.</param>
        public AquariumToken(String pTokenName, Kernel pKernel, int pWidth, int pHeight)
            : base(pTokenName)
        {
            mKernel = pKernel;      // Store reference to kernel.
            mHeight = pHeight;      // Height of the aquarium.
            mWidth = pWidth;        // Width of the aquarium.
        }

        #endregion
        
        #region Methods

        /* LEARNING PILL: Token default properties.
         * In the XNA Machinationis Ratio engine tokens have properties that define
         * how the behave and are visualized. Using the DefaultProperties method 
         *  it is possible to assign deafult values to the token's properties,
         * after the token has been created.
         */

        /// <summary>
        /// Setup default values for this token's porperties.
        /// </summary>
        protected override void DefaultProperties()
        {
            // Specify which image should be associated to this token, assigning
            // the name of the graphic asset to be used ("AquariumVisuals" in this case)
            // to the property 'GraphicProperties.AssetID' of the token.
            this.GraphicProperties.AssetID = "AquariumVisuals";

            AquariumMind myMind = new AquariumMind(this);   // Create mind, implicitly associating it to the token.


            mMind = myMind;         // Store explicit reference to mind being used.
            mMind.Aquarium = this;  // Provide to mind explicit reference to Aquarium.
        }

        /// <summary>
        /// Indicate whether a game object in the aquarium has reached the aquarium's
        /// horizontal boundaries.
        /// </summary>
        /// <param name="pObject">Object to check.</param>
        /// <returns>Result of the check.</returns>
        public bool ReachedHorizontalBoundary(GameObject pObject)
        {
            // Check if the absolute value of the horizontal distance between the center of
            // aquarium and the object is greater than half the width of the aquarium,
            // which means that center of the object has reached the horizontal boundary
            // of the aquarium.
            if (Math.Abs(pObject.Position.X - this.Position.X) >= (this.mWidth / 2))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Indicate whether a game object in the aquarium has reached the aquarium's
        /// horizontal boundaries.
        /// </summary>
        /// <param name="pObject">Object to check.</param>
        /// <returns>Result of the check.</returns>
        public bool ReachedVerticalBoundary(GameObject pObject)
        {
            // Check if the absolute value of the horizontal distance between the center of
            // aquarium and the object is greater than half the width of the aquarium,
            // which means that center of the object has reached the horizontal boundary
            // of the aquarium.
            if (Math.Abs(pObject.Position.Y - this.Position.Y) >= (this.mHeight / 2))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Remove chicken leg from aquarium.
        /// </summary>
        public void RemoveChickenLeg()
        {
            this.mKernel.Scene.Remove(mChickenLeg);
            mChickenLeg = null;
        }

        #endregion
    }
}