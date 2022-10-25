const addMessage = (item) => {
  return {
    resultString: item.resultString,
    resultCode: item.resultCode,
  };
};

const fromJson = (item) => {
  return {
    id: item.id,
    title: item.title,
    content: item.content,
    read: item.read,
    recipientId: item.recipientId,
    recipient: item.recipient,
    senderId: item.senderId,
    url: item.url,
  };
};

const baseJson = () => {
  return {
    id: null,
    title: null,
    content: null,
    read: null,
    recipientId: null,
    recipient: null,
    senderId: null,
    url: null,
  };
};

const toListModel = (items) => {
  if (items.lenght > 0) {
    let data = [];
    items.map((value, index) => {
      data.push(fromJson(value));
    });
    return data ?? [];
  }
  return [];
};

export const notifyModel = { addMessage, fromJson, toListModel, baseJson };
