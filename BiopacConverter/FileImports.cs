/*
Copyright 2005-2013 BIOPAC Systems, Inc.

This software is provided 'as-is', without any express or implied warranty.
In no event will BIOPAC Systems, Inc. or BIOPAC Systems, Inc. employees be 
held liable for any damages arising from the use of this software.

Permission is granted to anyone to use this software for any purpose, 
including commercial applications, and to alter it and redistribute it 
freely, subject to the following restrictions:

1. The origin of this software must not be misrepresented; you must not 
claim that you wrote the original software. If you use this software in a 
product, an acknowledgment (see the following) in the product documentation
is required.

Portions Copyright 2005-2013 BIOPAC Systems, Inc.

2. Altered source versions must be plainly marked as such, and must not be 
misrepresented as being the original software.

3. This notice may not be removed or altered from any source distribution.
*/

using System.Runtime.InteropServices;

namespace Biopac.API.AcqFile
{
	/// <summary>
	/// C# translation of acqfile.h. A language binding for acqfile.dll is created when compiled
	/// Supports File API v3.1.0 for Windows
	/// See BIOPAC File API documentation for full documentation
	/// </summary>
	public class AcqFileInfo
	{

        /**
         * Number of bytes for channel labels starting with file version 410R3
         * Maximum length of a channel label, in bytes.  Channel labels in files
         * will be limited to this length.  1 byte must be reserved for NULL terminator
         * storage.  All strings are kept UTF8 encoded.
         */
        public const int k410R3ChannelLabelLength = 1024;


        /**
         * Number of bytes for channel units starting with file version 410R3
         * Maximum length of vertical channel units, in bytes.  Channel units in files
         * will be limited to this length.  1 byte must be reserved for NULL terminator
         * storage.  All strings are kept UTF8 encoded.
         */
        public const int k410R3ChannelUnitLength = 512;


        // List of supported versions of .ACQ files
		public enum ACQFILEVER 
        {
	        // MUG 06/01/2007 - support of MAC BSL files added and tested
	        BSLMAC370 = 37,		// Biopac Student Lab (and PRO) 370 for MAC
	        ACK370 = 38,		// AcqKnwowledge 370 for PC
	        ACK373 = 39,		// AcqKnwowledge 373 for PC
	        ACK381 = 41,		// AcqKnwowledge 381 for PC
	        BSL370 = 42,		// Biopac Student Lab (and PRO) 370 for PC
	        ACK382 = 43,		// AcqKnwowledge 382 for PC
	        ACK390 = 45,			// AcqKnwowledge 390 for PC
	        // [mug] 07/16/2009
	        // BIOPAC File API v 3.0.0
	        ACK41 = 83,			//!< Version 4.1.0 Rev 3, channel label storage extended to 1024 bytes, units to 256 bytes. (IDFileVersion410R3)

            // {mug} 05/16/2011
            // BIOPAC FIle API v 3.1.0
	        ACK41_4 = 84,		// Veresion 4.1.0 Rev 4, creator handle modified to hold Run Macro calculation channel presets. (IDFileVersion410R4) [ed] 1/5/10
	        ACK412 = 85,		// Version 4.1.2, BSL Qt.  Used to indicate version where dotSize() holds line thickness for step/line plotting modes.  files earlier than this version will have the plot thickness set to 1 for step and line mode channels.
	        ACK412_R1 = 86,	    // Version 4.1.2 Rev1, BSL Qt, journal window bounds/dock widget size 
	        ACK412_R2 = 87,	    // Version 4.1.2 Rev2, BSL Qt, sequential event hotkey labels and predefined segment labels for appended segments added
            ACK420_R12 = 108,    // ACQ v 4.2.0 Rev 12

	        ACK430	    = 109,	// BSL/ACQ, Arbitrary number of graph channels 
	        ACK430_R1	= 110,	// BSL/ACQ, Hardware name 
						        // 111 is an internal version, should not be released to customers
	        ACK430_R4	= 112,	// BSL/ACQ, horizontal/vertical axis precision
	        ACK430_R5	= 113,	// BSL/ACQ, journal visibility flag

	        // {mug} 2013/02/07
	        // BAPI-7, Marker text extraction must be updated for BSL 4.0.1 with timestamps
	        ACQ430_R7	= 115,	// BSL/ACQ, Add timestamp to marker struct [AK] 03/18/2012

	        ACK430_R13	= 121,	// Format updates for AcqKnowledge 4.3.0
	        ACQ440		= 122,	// Version 4.4.0 Split view panel section [AK] 11/02/12
	        ACK440_R2	= 124	// Format updates for BSL 4.0.1
        }


        //! Data structure that describes an ACQ file
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ACQFILESTRUCT
        {
            public byte bPCFile; /**< flag to differ Acq 3.x for Win file and Acq 4.1 files. if TRUE - Acq 39x file, FALSE - ACQ 4.1 file */
	        public int numChannels; /**< number of channels */
            public int mpHeaderLen; /**< length of the MP device header in bytes */
            public int mpDevID;  /**< ID of the MP device that recorded the data (currently it is always set to zero) */
            public int hFile;	/**< handle to the ACQ file */
            public int version;	/**< ACQ file version */
            public int fileHeaderLen; /**< length of the file information header in bytes*/
            public int chHeaderLen; /**< length of the channel information header in bytes */
            public int dataOffset; /**< offset of raw data from the beginning of the file in bytes */
            public int markerLen;	/**< total length of all marker data in bytes */
            public int numMarkers; /**< total number of markers */
            public int markerOffset; /**< offset of the first marker from the beginning of the file in bytes */
            public int journalLen; /**< length of the Journal text (not including the null character) */
            public double sampleRate; /**< sample rate in msec/sample */
            public int dataViewSectionLength;  /**< length of the Data View section in bytes */
            public int splitPanelSectionLength; /**< length of the Split panel section in bytes */
        };

        //! Data structure that describes a channel in an ACQ file
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct CHANNELSTRUCT_A
        {
            public int num; /**< channel number */
            public int index; /**< index of the channel as it appears in the file */
            public int numSamples; /**< number of samples for each channel (this should be the same for all channels) */
            public double scale;	/**< scale of the raw data */
            public double offset; /**< offset of the raw data */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]   /**< text label of the channel */
            public byte[] label;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]   /**< units of the channel */
            public byte[] units;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = k410R3ChannelLabelLength)]   /**< channel label/description text, for version 410R3 and higher.  1024 bytes. */
            public byte[] newComTxt;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = k410R3ChannelUnitLength)]   /**< amplitude units, for version 410R3 and higher.  512 bytes. */
            public byte[] newUnitsTxt;
        };

        //! Data structure that describes a channel in an ACQ file (UNICODE)
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct CHANNELSTRUCT_W
        {
            public int num; /**< channel number */
            public int index; /**< index of the channel as it appears in the file */
            public int numSamples; /**< number of samples for each channel (this should be the same for all channels) */
            public double scale;	/**< scale of the raw data */
            public double offset; /**< offset of the raw data */
            public unsafe fixed byte label[40 * 2];
            public unsafe fixed byte units[20 * 2];
            public unsafe fixed byte newComTxt[k410R3ChannelLabelLength * 2];
            public unsafe fixed byte newUnitsTxt[k410R3ChannelUnitLength * 2];
        };

        public struct MARKERSTRUCT
        {
            public int textLength; /**< length of the marker text (not including the null character) */
            public int location; /**< location of the marker in number of samples  */
            public int index;	/**< index of the marker as it appears in the file */
            public int textOffset; /**< offset of the marker text from the beginning of the file in bytes */
        };


		[DllImport(@"acqfile.dll")]
        public unsafe static extern bool initACQFile_A(string szFilename, ref ACQFILESTRUCT ACQstruct);

		[DllImport(@"acqfile.dll")]
		public unsafe static extern bool initACQFile_W(string szFilename, ref ACQFILESTRUCT ACQstruct);

		[DllImport(@"acqfile.dll")]
        public unsafe static extern bool closeACQFile(ref ACQFILESTRUCT ACQstruct);

		[DllImport(@"acqfile.dll")]
        public unsafe static extern bool getChannelInfo_A(int iChanNumber, ref ACQFILESTRUCT ACQstruct, ref CHANNELSTRUCT_A ChanStruct);

		[DllImport(@"acqfile.dll")]
        public unsafe static extern bool getChannelInfo_W(int iChanNumber, ref ACQFILESTRUCT ACQstruct, ref CHANNELSTRUCT_W ChanStruct);

		[DllImport(@"acqfile.dll")]
        public unsafe static extern bool getSampleSegment(ref ACQFILESTRUCT ACQstruct, ref CHANNELSTRUCT_A ChanStruct, double [] values, int start, int end);
        
		[DllImport(@"acqfile.dll")]
        public unsafe static extern bool getAllSamples(ref ACQFILESTRUCT ACQstruct, ref CHANNELSTRUCT_A ChanStruct, double [] values);
        
		[DllImport(@"acqfile.dll")]
        public unsafe static extern bool getSample(ref ACQFILESTRUCT ACQstruct, ref CHANNELSTRUCT_A ChanStruct, double [] values, int iIndex);
        
		[DllImport(@"acqfile.dll")]
        public unsafe static extern bool getTimeSlice(ref ACQFILESTRUCT ACQstruct, ref CHANNELSTRUCT_A ChanStruct, double [] values, int start, int end);

		[DllImport(@"acqfile.dll")]
        public unsafe static extern bool getJournalText_A(ref ACQFILESTRUCT ACQstruct, byte[] szText);
        
		[DllImport(@"acqfile.dll")]
        public unsafe static extern bool getJournalText_W(ref ACQFILESTRUCT ACQstruct, byte[] szText);

  		[DllImport(@"acqfile.dll")]
        public unsafe static extern bool getMarkerInfo(int iIndex, ref ACQFILESTRUCT ACQstruct, ref MARKERSTRUCT MarkStruct);

  		[DllImport(@"acqfile.dll")]
        public unsafe static extern bool getMarkerText_A(ref ACQFILESTRUCT ACQstruct, ref MARKERSTRUCT MarkStruct, byte[] szText);

  		[DllImport(@"acqfile.dll")]
        public unsafe static extern bool getMarkerText_W(ref ACQFILESTRUCT ACQstruct, ref MARKERSTRUCT MarkStruct, byte[] szText);
	}
}
