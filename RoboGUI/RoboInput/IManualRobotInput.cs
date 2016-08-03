using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboInput
{
    public delegate void ScanArea();

    public delegate void ScanSingleSegment();

    public delegate void StopMovement();

    /// <summary>
    /// </summary>
    /// <param name="released">Indicates whether the button was pressed or released.</param>
    /// <param name="speed"></param>
    public delegate void TurnLeft(bool released);

    /// <summary>
    /// </summary>
    /// <param name="released">Indicates whether the button was pressed or released.</param>
    /// <param name="speed"></param>
    public delegate void TurnRight(bool released);

    /// <summary>
    /// </summary>
    /// <param name="released">Indicates whether the button was pressed or released.</param>
    /// <param name="speed"></param>
    public delegate void MoveForward(bool released);

    /// <summary>
    /// </summary>
    /// <param name="released">Indicates whether the button was pressed or released.</param>
    /// <param name="speed"></param>
    public delegate void MoveBackward(bool released);

    public interface IManualRobotInput
    {
        event ScanArea OnScanArea;

        event StopMovement OnStopMovement;

        event TurnLeft OnTurnLeft;

        event TurnRight OnTurnRight;

        event MoveForward OnMoveForward;

        event MoveBackward OnMoveBackward;

        void ListenToInput(bool listen);
    }
}
