using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    internal class HighlightConverterer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Hightlight term = (Hightlight)value;
            if (term != null)
            {
                writer.WriteStartObject();
                if (term.fields != null)
                {
                    writer.WritePropertyName("fields");
                    writer.WriteStartObject();
                    foreach (HightlightField hightlightField in term.fields)
                    {
                        writer.WritePropertyName(hightlightField.name);
                        writer.WriteStartObject();


                        if (!string.IsNullOrEmpty(hightlightField.fragment_size))
                        {
                            writer.WritePropertyName("fragment_size");
                            writer.WriteValue(hightlightField.fragment_size);
                        }
                        if (!string.IsNullOrEmpty(hightlightField.number_of_fragments))
                        {
                            writer.WritePropertyName("number_of_fragments");
                            writer.WriteValue(hightlightField.number_of_fragments);
                         }

                        
                        if (!string.IsNullOrEmpty(hightlightField.fragment_offset))
                        {
                            writer.WritePropertyName("fragment_offset");
                            writer.WriteValue(hightlightField.fragment_offset);
                        }
                        if (!string.IsNullOrEmpty(hightlightField.boundary_chars))
                        {
                            writer.WritePropertyName("boundary_chars");
                            writer.WriteValue(hightlightField.boundary_chars);
                        }
                        if (!hightlightField.boundary_max_size.Equals(default(int)))
                        {
                            writer.WritePropertyName("boundary_max_size");
                            writer.WriteValue(hightlightField.boundary_max_size);
                        }
                        if (!string.IsNullOrEmpty(hightlightField.order))
                        {
                            writer.WritePropertyName("order");
                            writer.WriteValue(hightlightField.order);
                        }
                        if (!string.IsNullOrEmpty(hightlightField.post_tags))
                        {
                            writer.WritePropertyName("post_tags");
                            writer.WriteValue(hightlightField.post_tags);
                        }
                        if (!string.IsNullOrEmpty(hightlightField.pre_tags))
                        {
                            writer.WritePropertyName("pre_tags");
                            writer.WriteValue(hightlightField.pre_tags);
                        }
                        if (!hightlightField.require_field_match.Equals(default(bool)))
                        {
                            writer.WritePropertyName("require_field_match");
                            writer.WriteValue(hightlightField.require_field_match);
                        }
                        if (!string.IsNullOrEmpty(hightlightField.tag_schema))
                        {
                            writer.WritePropertyName("tag_schema");
                            writer.WriteValue(hightlightField.tag_schema);
                        }

                        writer.WriteEndObject();
                    }
                    writer.WriteEndObject();
                }

                //other golobal property

                if (!string.IsNullOrEmpty(term.fragment_size))
                {
                    writer.WritePropertyName("fragment_size");
                    writer.WriteValue(term.fragment_size);
                }
                if (!string.IsNullOrEmpty(term.number_of_fragments))
                {
                    writer.WritePropertyName("number_of_fragments");
                    writer.WriteValue(term.number_of_fragments);
                }
                if (!string.IsNullOrEmpty(term.order))
                {
                    writer.WritePropertyName("order");
                    writer.WriteValue(term.order);
                } 
                if (!string.IsNullOrEmpty(term.tag_schema))
                {
                    writer.WritePropertyName("tag_schema");
                    writer.WriteValue(term.tag_schema);
                }

                writer.WriteEndObject();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Hightlight).IsAssignableFrom(objectType);
        }
    }
}