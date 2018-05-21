/* Copyright (c) 2017 Rick (rick 'at' gibbed 'dot' us)
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 * 
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 * 
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

using System.Runtime.InteropServices;

namespace Gibbed.IO
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct OverlapSingle
    {
        [FieldOffset(0)]
        private float F;

        [FieldOffset(0)]
        private uint U;

        public OverlapSingle(float f)
        {
            this.U = default(uint);
            this.F = f;
        }

        public OverlapSingle(uint u)
        {
            this.F = default(float);
            this.U = u;
        }

        public float AsF
        {
            get { return this.F; }
            set { this.F = value; }
        }

        public uint AsU
        {
            get { return this.U; }
            set { this.U = value; }
        }
    }
}
