/* This driver uses the FieldTrip buffer open source library. 
 * See http://www.ru.nl/fcdonders/fieldtrip for details.
 */

#ifndef __OpenViBE_AcquisitionServer_CDriverFieldtrip_H__
#define __OpenViBE_AcquisitionServer_CDriverFieldtrip_H__

#include "ovasCHeader.h"
#include "ov_types.h"
#include <string>
#include "fieldtrip_buffer\src\message.h"
#include "FunctionCallback.h"
// for GET_CPU_TIME
//#include <iostream>
//#include <fstream>
using namespace std;

namespace OpenViBEAcquisitionServer
{
	/**
	 * \class CDriverFieldtrip
	 * \author Amelie Serpollet (CEA/LETI/CLINATEC)
	 * \author Anton Andreev
	 * \date Mon May 23 09:48:21 2011
	 * \brief The CDriverFieldtrip allows the acquisition server to acquire data from a Fieldtrip buffer.
	 *
	 * TODO: details
	 *
	 */
	class FieldTripDriverNative
	{
	public:

		FieldTripDriverNative();
		FieldTripDriverNative(string hostname, int port);
		~FieldTripDriverNative(void);
		
		bool initialize(const OpenViBE::uint32 ui32SampleCountPerSentBlock);
		OpenViBE::boolean uninitialize(void);

		OpenViBE::boolean start(void);
		OpenViBE::boolean stop(void);
		OpenViBE::boolean loop(CallbackType);

		OpenViBE::boolean configure(void);
		const OpenViBEAcquisitionServer::IHeader* getHeader(void) { return &m_oHeader; }

		bool FoundChannelNames(){ return l_bFoundChannelNames;}

		std::string GetChannelName(const OpenViBE::uint32 index)
		{
			if (index>=0 && index<m_oHeader.getChannelCount())
			{
				return m_oHeader.getChannelName(index);
			}
			return "";
		}

		double GetSamplingFrequency()
		{
			if (m_oHeader.isSamplingFrequencySet())
			return static_cast<double>(m_oHeader.getSamplingFrequency());
			else return -1;
		}

	protected:

		OpenViBEAcquisitionServer::CHeader m_oHeader;

		OpenViBE::uint32 m_ui32SampleCountPerSentBlock;
		OpenViBE::float32* m_pSample;

        bool requestHeader();
        OpenViBE::int32 requestChunk(/*OpenViBE::CStimulationSet& oStimulationSet*/);

        OpenViBE::uint32 m_ui32DataType;

	private:
        // Connection to Fieldtrip buffer
        string m_sHostName;
        OpenViBE::uint32  m_ui32PortNumber;
        OpenViBE::int32   m_i32ConnectionID;
        OpenViBE::uint32  m_ui32MinSamples;

        // Avoid frequent memory allocation
        message_t* m_pWaitData_Request;
        message_t* m_pGetData_Request;

        OpenViBE::uint32 m_ui32TotalSampleCount;
        OpenViBE::uint32 m_ui32WaitingTimeMs;

        bool m_bFirstGetDataRequest;

        bool m_bCorrectNonIntegerSR; // ???
        OpenViBE::float64 m_f64RealSamplingRate;
        OpenViBE::float64 m_f64DiffPerSample; // ???
        OpenViBE::float64 m_f64DriftSinceLastCorrection;

		OpenViBE::boolean l_bFoundChannelNames;
        // edges detection for "get cpu time"
        /*FILE* m_myfile;
        bool m_bWasDetected;
        bool m_bGetCpuTime;
        string m_sMeasureFolder;
        OpenViBE::float64 m_f64DetectionThreshold;
        bool m_bDetectionHigher;
        OpenViBE::uint32 m_ui32DetectionChannel;*/

        // count time lost for "get cpu time" :
        /*OpenViBE::float64 m_f64mesureLostTime;
        OpenViBE::uint32 m_ui32mesureNumber;*/
        
	};
};

#endif // __OpenViBE_AcquisitionServer_CDriverFieldtrip_H__