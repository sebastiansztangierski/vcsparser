﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vcsparser.core
{
    public class SonarMeasuresJson
    {
        public SonarMeasuresJson()
        {
            this.Measures = new List<IMeasure>();
            this.MeasuresProject = new List<IMeasure>();
            this.Metrics = new List<Metric>();
        }

        [JsonProperty("file-measures")]
        public List<IMeasure> Measures
        {
            get; set;
        }

        [JsonProperty("project-measures")]
        public List<IMeasure> MeasuresProject {
            get; set;
        }

        private Dictionary<string, Dictionary<string, IMeasure>> measureFileIndex = new Dictionary<string, Dictionary<string, IMeasure>>();
        private Dictionary<string, IMeasure> measureProjectIndex = new Dictionary<string, IMeasure>();

        [JsonProperty("metrics")]
        public List<Metric> Metrics { get; set; }

        public IMeasure FindFileMeasure(string metricKey, string fileName)
        {
            if (!measureFileIndex.ContainsKey(metricKey))
                return null;
            if (!measureFileIndex[metricKey].ContainsKey(fileName))
                return null;
            return measureFileIndex[metricKey][fileName];            
        }

        public void AddFileMeasure(IMeasure measure)
        {
            this.Measures.Add(measure);

            if (!measureFileIndex.ContainsKey(measure.MetricKey))
                measureFileIndex.Add(measure.MetricKey, new Dictionary<string, IMeasure>());

            if (!measureFileIndex[measure.MetricKey].ContainsKey(measure.File))
                measureFileIndex[measure.MetricKey].Add(measure.File, measure);
            else
                throw new Exception("Measure already exists.");
        }

        public IMeasure FindProjectMeasure(string metricKey)
        {
            if (!measureProjectIndex.ContainsKey(metricKey))
                return null;
            return measureProjectIndex[metricKey];
        }

        public void AddProjectMeasure(IMeasure measure)
        {
            this.MeasuresProject.Add(measure);

            if (!measureProjectIndex.ContainsKey(measure.MetricKey))
                measureProjectIndex.Add(measure.MetricKey, measure);
            else
                throw new Exception("Measure already exists.");
        }
    }
}
