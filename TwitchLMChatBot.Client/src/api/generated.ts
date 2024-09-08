/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

/** @format int32 */
export enum AccountType {
  Value0 = 0,
  Value1 = 1,
}

export interface AccountUser {
  id?: string | null;
  login?: string | null;
  displayName?: string | null;
  /** @format date-time */
  createdAt?: string;
  type?: string | null;
  broadcasterType?: string | null;
  description?: string | null;
  profileImageUrl?: string | null;
  offlineImageUrl?: string | null;
  /** @format int64 */
  viewCount?: number;
  email?: string | null;
}

export interface Command {
  /** @format int32 */
  id?: number;
  name?: string | null;
  response?: string | null;
  isEnabled?: boolean;
}

export interface CreateCommandRequest {
  name?: string | null;
  response?: string | null;
}

export interface CreatePersonalityRequest {
  personalityName?: string | null;
  instructions?: string | null;
}

export interface CreateProviderRequest {
  providerType?: ProviderType;
  providerName?: string | null;
  baseUrl?: string | null;
  apiKey?: string | null;
}

export interface Personality {
  /** @format int32 */
  id?: number;
  name?: string | null;
  model?: string | null;
  instructions?: string | null;
  isDefault?: boolean;
  provider?: Provider;
  prividerName?: string | null;
}

export interface ProblemDetails {
  type?: string | null;
  title?: string | null;
  /** @format int32 */
  status?: number | null;
  detail?: string | null;
  instance?: string | null;
  [key: string]: any;
}

export interface Provider {
  /** @format int32 */
  id?: number;
  name?: string | null;
  type?: ProviderType;
  baseUrl?: string | null;
  apiKey?: string | null;
  isDefault?: boolean;
  typeName?: string | null;
}

/** @format int32 */
export enum ProviderType {
  Value0 = 0,
  Value1 = 1,
}

export interface SetAccessControlRequest {
  unrestricted?: boolean;
  followers?: boolean;
  subscribers?: boolean;
  moderators?: boolean;
  vips?: boolean;
}

export interface UpdateCommandRequest {
  name?: string | null;
  response?: string | null;
}

export interface UpdatePersonalityRequest {
  personalityName?: string | null;
  instructions?: string | null;
}

export type QueryParamsType = Record<string | number, any>;
export type ResponseFormat = keyof Omit<Body, "body" | "bodyUsed">;

export interface FullRequestParams extends Omit<RequestInit, "body"> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean;
  /** request path */
  path: string;
  /** content type of request body */
  type?: ContentType;
  /** query params */
  query?: QueryParamsType;
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseFormat;
  /** request body */
  body?: unknown;
  /** base url */
  baseUrl?: string;
  /** request cancellation token */
  cancelToken?: CancelToken;
}

export type RequestParams = Omit<FullRequestParams, "body" | "method" | "query" | "path">;

export interface ApiConfig<SecurityDataType = unknown> {
  baseUrl?: string;
  baseApiParams?: Omit<RequestParams, "baseUrl" | "cancelToken" | "signal">;
  securityWorker?: (securityData: SecurityDataType | null) => Promise<RequestParams | void> | RequestParams | void;
  customFetch?: typeof fetch;
}

export interface HttpResponse<D extends unknown, E extends unknown = unknown> extends Response {
  data: D;
  error: E;
}

type CancelToken = Symbol | string | number;

export enum ContentType {
  Json = "application/json",
  FormData = "multipart/form-data",
  UrlEncoded = "application/x-www-form-urlencoded",
  Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
  public baseUrl: string = "";
  private securityData: SecurityDataType | null = null;
  private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"];
  private abortControllers = new Map<CancelToken, AbortController>();
  private customFetch = (...fetchParams: Parameters<typeof fetch>) => fetch(...fetchParams);

  private baseApiParams: RequestParams = {
    credentials: "same-origin",
    headers: {},
    redirect: "follow",
    referrerPolicy: "no-referrer",
  };

  constructor(apiConfig: ApiConfig<SecurityDataType> = {}) {
    Object.assign(this, apiConfig);
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data;
  };

  protected encodeQueryParam(key: string, value: any) {
    const encodedKey = encodeURIComponent(key);
    return `${encodedKey}=${encodeURIComponent(typeof value === "number" ? value : `${value}`)}`;
  }

  protected addQueryParam(query: QueryParamsType, key: string) {
    return this.encodeQueryParam(key, query[key]);
  }

  protected addArrayQueryParam(query: QueryParamsType, key: string) {
    const value = query[key];
    return value.map((v: any) => this.encodeQueryParam(key, v)).join("&");
  }

  protected toQueryString(rawQuery?: QueryParamsType): string {
    const query = rawQuery || {};
    const keys = Object.keys(query).filter((key) => "undefined" !== typeof query[key]);
    return keys
      .map((key) => (Array.isArray(query[key]) ? this.addArrayQueryParam(query, key) : this.addQueryParam(query, key)))
      .join("&");
  }

  protected addQueryParams(rawQuery?: QueryParamsType): string {
    const queryString = this.toQueryString(rawQuery);
    return queryString ? `?${queryString}` : "";
  }

  private contentFormatters: Record<ContentType, (input: any) => any> = {
    [ContentType.Json]: (input: any) =>
      input !== null && (typeof input === "object" || typeof input === "string") ? JSON.stringify(input) : input,
    [ContentType.Text]: (input: any) => (input !== null && typeof input !== "string" ? JSON.stringify(input) : input),
    [ContentType.FormData]: (input: any) =>
      Object.keys(input || {}).reduce((formData, key) => {
        const property = input[key];
        formData.append(
          key,
          property instanceof Blob
            ? property
            : typeof property === "object" && property !== null
              ? JSON.stringify(property)
              : `${property}`,
        );
        return formData;
      }, new FormData()),
    [ContentType.UrlEncoded]: (input: any) => this.toQueryString(input),
  };

  protected mergeRequestParams(params1: RequestParams, params2?: RequestParams): RequestParams {
    return {
      ...this.baseApiParams,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...(this.baseApiParams.headers || {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      },
    };
  }

  protected createAbortSignal = (cancelToken: CancelToken): AbortSignal | undefined => {
    if (this.abortControllers.has(cancelToken)) {
      const abortController = this.abortControllers.get(cancelToken);
      if (abortController) {
        return abortController.signal;
      }
      return void 0;
    }

    const abortController = new AbortController();
    this.abortControllers.set(cancelToken, abortController);
    return abortController.signal;
  };

  public abortRequest = (cancelToken: CancelToken) => {
    const abortController = this.abortControllers.get(cancelToken);

    if (abortController) {
      abortController.abort();
      this.abortControllers.delete(cancelToken);
    }
  };

  public request = async <T = any, E = any>({
    body,
    secure,
    path,
    type,
    query,
    format,
    baseUrl,
    cancelToken,
    ...params
  }: FullRequestParams): Promise<HttpResponse<T, E>> => {
    const secureParams =
      ((typeof secure === "boolean" ? secure : this.baseApiParams.secure) &&
        this.securityWorker &&
        (await this.securityWorker(this.securityData))) ||
      {};
    const requestParams = this.mergeRequestParams(params, secureParams);
    const queryString = query && this.toQueryString(query);
    const payloadFormatter = this.contentFormatters[type || ContentType.Json];
    const responseFormat = format || requestParams.format;

    return this.customFetch(`${baseUrl || this.baseUrl || ""}${path}${queryString ? `?${queryString}` : ""}`, {
      ...requestParams,
      headers: {
        ...(requestParams.headers || {}),
        ...(type && type !== ContentType.FormData ? { "Content-Type": type } : {}),
      },
      signal: (cancelToken ? this.createAbortSignal(cancelToken) : requestParams.signal) || null,
      body: typeof body === "undefined" || body === null ? null : payloadFormatter(body),
    }).then(async (response) => {
      const r = response.clone() as HttpResponse<T, E>;
      r.data = null as unknown as T;
      r.error = null as unknown as E;

      const data = !responseFormat
        ? r
        : await response[responseFormat]()
            .then((data) => {
              if (r.ok) {
                r.data = data;
              } else {
                r.error = data;
              }
              return r;
            })
            .catch((e) => {
              r.error = e;
              return r;
            });

      if (cancelToken) {
        this.abortControllers.delete(cancelToken);
      }

      if (!response.ok) throw data;
      return data;
    });
  };
}

/**
 * @title TwitchLMChatBot.Server
 * @version 1.0
 */
export class Api<SecurityDataType extends unknown> extends HttpClient<SecurityDataType> {
  api = {
    /**
     * No description
     *
     * @tags BotAccount
     * @name BotAccountList
     * @request GET:/api/bot-account
     */
    botAccountList: (params: RequestParams = {}) =>
      this.request<AccountUser, ProblemDetails>({
        path: `/api/bot-account`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags BotAccount
     * @name BotAccountDelete
     * @request DELETE:/api/bot-account
     */
    botAccountDelete: (params: RequestParams = {}) =>
      this.request<AccountUser, any>({
        path: `/api/bot-account`,
        method: "DELETE",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags BroadcasterAccount
     * @name BroadcasterAccountList
     * @request GET:/api/broadcaster-account
     */
    broadcasterAccountList: (params: RequestParams = {}) =>
      this.request<AccountUser, ProblemDetails>({
        path: `/api/broadcaster-account`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags BroadcasterAccount
     * @name BroadcasterAccountDelete
     * @request DELETE:/api/broadcaster-account
     */
    broadcasterAccountDelete: (params: RequestParams = {}) =>
      this.request<AccountUser, any>({
        path: `/api/broadcaster-account`,
        method: "DELETE",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Command
     * @name ListCommands
     * @request GET:/api/commands
     */
    listCommands: (params: RequestParams = {}) =>
      this.request<Command[], any>({
        path: `/api/commands`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Command
     * @name CreateCommand
     * @request POST:/api/commands
     */
    createCommand: (data: CreateCommandRequest, params: RequestParams = {}) =>
      this.request<Command, any>({
        path: `/api/commands`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Command
     * @name FindCommand
     * @request GET:/api/commands/{id}
     */
    findCommand: (id: number, params: RequestParams = {}) =>
      this.request<Command, any>({
        path: `/api/commands/${id}`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Command
     * @name UpdateCommand
     * @request PUT:/api/commands/{id}
     */
    updateCommand: (id: number, data: UpdateCommandRequest, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/commands/${id}`,
        method: "PUT",
        body: data,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Command
     * @name DeleteCommand
     * @request DELETE:/api/commands/{id}
     */
    deleteCommand: (id: number, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/commands/${id}`,
        method: "DELETE",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Permissions
     * @name GetPermissions
     * @request GET:/api/permissions
     */
    getPermissions: (params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/permissions`,
        method: "GET",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Permissions
     * @name SetPermissions
     * @request PUT:/api/permissions
     */
    setPermissions: (data: SetAccessControlRequest, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/permissions`,
        method: "PUT",
        body: data,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Personalities
     * @name GetPersonalities
     * @request GET:/api/personalities
     */
    getPersonalities: (params: RequestParams = {}) =>
      this.request<Personality[], any>({
        path: `/api/personalities`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Personalities
     * @name CreatePersonality
     * @request POST:/api/personalities
     */
    createPersonality: (data: CreatePersonalityRequest, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/personalities`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Personalities
     * @name SetDefaultPersonality
     * @request PUT:/api/personalities/{id}/default
     */
    setDefaultPersonality: (id: number, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/personalities/${id}/default`,
        method: "PUT",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Personalities
     * @name UpdatePersonality
     * @request PUT:/api/personalities/{id}
     */
    updatePersonality: (id: number, data: UpdatePersonalityRequest, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/personalities/${id}`,
        method: "PUT",
        body: data,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Personalities
     * @name DeletePersonality
     * @request DELETE:/api/personalities/{id}
     */
    deletePersonality: (id: number, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/personalities/${id}`,
        method: "DELETE",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Providers
     * @name ListProviders
     * @request GET:/api/providers
     */
    listProviders: (params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/providers`,
        method: "GET",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Providers
     * @name CreateProvider
     * @request POST:/api/providers
     */
    createProvider: (data: CreateProviderRequest, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/providers`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Providers
     * @name DeleteProvider
     * @request DELETE:/api/providers/{id}
     */
    deleteProvider: (id: number, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/providers/${id}`,
        method: "DELETE",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Providers
     * @name SetDefault
     * @request PUT:/api/providers/{id}/default
     */
    setDefault: (id: number, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/providers/${id}/default`,
        method: "PUT",
        ...params,
      }),
  };
  auth = {
    /**
     * No description
     *
     * @tags TwitchAuth
     * @name TwitchList
     * @request GET:/auth/twitch
     */
    twitchList: (
      query?: {
        accountType?: AccountType;
      },
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/auth/twitch`,
        method: "GET",
        query: query,
        ...params,
      }),

    /**
     * No description
     *
     * @tags TwitchAuth
     * @name TwitchCallbackList
     * @request GET:/auth/twitch/callback
     */
    twitchCallbackList: (
      query?: {
        code?: string;
        state?: string;
      },
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/auth/twitch/callback`,
        method: "GET",
        query: query,
        ...params,
      }),
  };
}
