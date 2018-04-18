﻿using System.Collections.Generic;
using JetBrains.Annotations;
using NewRelic.Agent.Core.DataTransport;

namespace NewRelic.Agent.Core.Configuration
{
	public class SecurityPoliciesConfiguration
	{
		public const string RecordSqlPolicyName = "record_sql";
		public const string AttributesIncludePolicyName = "attributes_include";
		public const string AllowRawExceptionMessagePolicyName = "allow_raw_exception_messages";
		public const string CustomEventsPolicyName = "custom_events";
		public const string CustomParametersPolicyName = "custom_parameters";
		public const string CustomInstrumentationEditorPolicyName = "custom_instrumentation_editor";

		private static readonly List<string> KnownPolicies = new List<string>()
		{
			RecordSqlPolicyName,
			AttributesIncludePolicyName,
			AllowRawExceptionMessagePolicyName,
			CustomEventsPolicyName,
			CustomParametersPolicyName,
			CustomInstrumentationEditorPolicyName
		};

		[NotNull]
		private readonly Dictionary<string, SecurityPolicy> _policies = new Dictionary<string, SecurityPolicy>();

		[CanBeNull]
		public SecurityPolicy RecordSql => _policies.ContainsKey(RecordSqlPolicyName) ? _policies[RecordSqlPolicyName] : null;

		[CanBeNull]
		public SecurityPolicy AttributesInclude => _policies.ContainsKey(AttributesIncludePolicyName) ? _policies[AttributesIncludePolicyName] : null;

		[CanBeNull]
		public SecurityPolicy AllowRawExceptionMessage => _policies.ContainsKey(AllowRawExceptionMessagePolicyName) ? _policies[AllowRawExceptionMessagePolicyName] : null;

		[CanBeNull]
		public SecurityPolicy CustomEvents => _policies.ContainsKey(CustomEventsPolicyName) ? _policies[CustomEventsPolicyName] : null;

		[CanBeNull]
		public SecurityPolicy CustomParameters => _policies.ContainsKey(CustomParametersPolicyName) ? _policies[CustomParametersPolicyName] : null;

		[CanBeNull]
		public SecurityPolicy CustomInstrumentationEditor => _policies.ContainsKey(CustomInstrumentationEditorPolicyName) ? _policies[CustomInstrumentationEditorPolicyName] : null;

		public SecurityPoliciesConfiguration() {}

		public SecurityPoliciesConfiguration([NotNull]Dictionary<string, SecurityPolicyState> policies)
		{
			foreach(var policy in policies)
			{
				_policies.Add(policy.Key, new SecurityPolicy(policy.Key, policy.Value.Enabled));
			}
		}

		public bool SecurityPolicyExistsFor(string securityPolicyName)
		{
			return _policies.ContainsKey(securityPolicyName);
		}

		public static List<string> GetMissingExpectedSeverPolicyNames(Dictionary<string, SecurityPolicyState> policies)
		{
			var unknownPolicies = new List<string>();

			foreach (var policyName in KnownPolicies)
			{
				if (!policies.ContainsKey(policyName))
				{
					unknownPolicies.Add(policyName);
				}
			}

			return unknownPolicies;
		}

		public static List<string> GetMissingRequiredPolicies(Dictionary<string, SecurityPolicyState> policies)
		{
			var missingRequiredPolicies = new List<string>();

			foreach (var policy in policies)
			{
				if (policy.Value.Required && !KnownPolicies.Contains(policy.Key))
				{
					missingRequiredPolicies.Add(policy.Key);
				}
			}

			return missingRequiredPolicies;
		}
	}
}
